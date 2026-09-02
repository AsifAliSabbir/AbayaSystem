using AbayaSystem.Core;
using AbayaSystem.Core.AbayaSystem.Core;
using AbayaSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AbayaSystem.Infrastructure
{
    public interface IWorkflowService
    {
        ItemStatus DetermineInitialStatus(OrderType orderType, bool buyFabricForExternal);
        ItemStatus DetermineNextStatusAfterFabricProcurement(OrderItem item);
        ItemStatus DetermineNextStatusAfterCutting(OrderType orderType, bool handEmbRequired);
        ItemStatus DetermineNextStatusAfterHalfStitching(OrderType orderType, bool handEmbRequired, bool rawFabricEmb);
        ItemStatus DetermineNextStatusAfterHalfStitchEmb(bool handEmbRequired);
        ItemStatus DetermineNextStatusAfterFullStitching();

        Task<bool> DispatchToExternalVendorAsync(
            int orderItemId,
            DateTime? expectedReturnDate = null,
            string? notes = null,
            int? dispatchedByWorkerId = null,
            int? branchId = null,
            string? orderId = null);

        Task<bool> MarkExternalVendorReturnedAsync(
            int externalVendorJobId,
            string? notes = null,
            int? receivedByWorkerId = null);

        Task<bool> TransitionStatusAsync(
            int orderItemId,
            ItemStatus newStatus,
            int? assignedWorkerId = null,
            string? notes = null,
            int? assignedHandEmbroidererId = null,
            int? branchId = null,
            string? orderId = null);
    }

    public class WorkflowService : IWorkflowService
    {
        private readonly BoutiqueDbContext _db;

        public WorkflowService(BoutiqueDbContext db)
        {
            _db = db;
        }

        // --- Workflow Decision Logic ---

        public ItemStatus DetermineInitialStatus(OrderType orderType, bool buyFabricForExternal)
        {
            if (orderType == OrderType.External && !buyFabricForExternal)
            {
                return ItemStatus.QueueExternalVendor;
            }
            return ItemStatus.ReadyForFabricProcurement;
        }

        public ItemStatus DetermineNextStatusAfterFabricProcurement(OrderItem item)
        {
            var isHybrid = item.TypeOfOrder == OrderType.Hybrid ||
                           item.ExternalWorker?.SupportedType == ExternalWorkerType.Hybrid;

            if (isHybrid)
            {
                return item.rawFabricEmb
                    ? ItemStatus.QueueRawFabricEmb
                    : ItemStatus.QueueCut;
            }

            if (item.TypeOfOrder == OrderType.External ||
                (item.TypeOfOrder == OrderType.Internal && item.ExternalWorkerId.HasValue))
            {
                return ItemStatus.QueueExternalVendor;
            }

            return ItemStatus.QueueCut;
        }

        public ItemStatus DetermineNextStatusAfterCutting(OrderType orderType, bool handEmbRequired)
        {
            if (handEmbRequired || orderType == OrderType.Hybrid)
            {
                return ItemStatus.QueueHalfStitching;
            }
            return ItemStatus.QueueFullStitching;
        }

        public ItemStatus DetermineNextStatusAfterFullStitching() => ItemStatus.ReadyAtWorkShop;

        public ItemStatus DetermineNextStatusAfterHalfStitching(OrderType orderType, bool handEmbRequired, bool rawFabricEmb)
        {
            if (orderType == OrderType.Hybrid)
            {
                return ItemStatus.QueueHalfStitchEmb;
            }

            if (handEmbRequired)
            {
                return ItemStatus.QueueHandEmbAssignment;
            }

            return ItemStatus.QueueFullStitching;
        }

        public ItemStatus DetermineNextStatusAfterHalfStitchEmb(bool handEmbRequired) =>
            handEmbRequired ? ItemStatus.QueueHandEmbAssignment : ItemStatus.QueueFullStitching;

        public async Task<bool> DispatchToExternalVendorAsync(
            int orderItemId,
            DateTime? expectedReturnDate = null,
            string? notes = null,
            int? dispatchedByWorkerId = null,
            int? branchId = null,
            string? orderId = null)
        {
            var item = branchId.HasValue && !string.IsNullOrWhiteSpace(orderId)
                ? await _db.OrderItems.FirstOrDefaultAsync(i => i.BranchId == branchId.Value && i.OrderId == orderId && i.OrderItemId == orderItemId)
                : await _db.OrderItems.FirstOrDefaultAsync(i => i.OrderItemId == orderItemId);
            if (item == null || !item.ExternalWorkerId.HasValue)
            {
                return false;
            }

            var stage = item.Status switch
            {
                ItemStatus.QueueExternalVendor => ExternalVendorJobStage.FullExternalProduction,
                ItemStatus.QueueRawFabricEmb => ExternalVendorJobStage.RawFabricEmbroidery,
                ItemStatus.QueueHalfStitchEmb => ExternalVendorJobStage.HalfStitchEmbroidery,
                _ => (ExternalVendorJobStage?)null
            };

            if (!stage.HasValue)
            {
                return false;
            }

            var nextStatus = stage.Value switch
            {
                ExternalVendorJobStage.FullExternalProduction => ItemStatus.OutWithExternalVendor,
                ExternalVendorJobStage.RawFabricEmbroidery => ItemStatus.OutForRawFabricEmb,
                _ => ItemStatus.OutForHalfStitchEmb
            };

            var previousStatus = item.Status;
            item.Status = nextStatus;
            _db.ExternalVendorJobs.Add(new ExternalVendorJob
            {
                BranchId = item.BranchId,
                OrderId = item.OrderId,
                OrderItemId = item.OrderItemId,
                ExternalWorkerId = item.ExternalWorkerId.Value,
                Stage = stage.Value,
                Status = ExternalVendorJobStatus.Dispatched,
                DispatchedAt = DateTime.UtcNow,
                ExpectedReturnDate = expectedReturnDate,
                DispatchNotes = notes ?? string.Empty,
                DispatchedByWorkerId = dispatchedByWorkerId
            });

            AddStatusLog(item, previousStatus, nextStatus, item.ExternalWorkerId, notes);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkExternalVendorReturnedAsync(
            int externalVendorJobId,
            string? notes = null,
            int? receivedByWorkerId = null)
        {
            var job = await _db.ExternalVendorJobs
                .Include(j => j.OrderItem)
                .FirstOrDefaultAsync(j => j.ExternalVendorJobId == externalVendorJobId);

            if (job?.OrderItem == null || job.Status != ExternalVendorJobStatus.Dispatched)
            {
                return false;
            }

            var item = job.OrderItem;
            var nextStatus = job.Stage switch
            {
                ExternalVendorJobStage.RawFabricEmbroidery => ItemStatus.QueueCut,
                ExternalVendorJobStage.HalfStitchEmbroidery => item.HandEmbRequired
                    ? ItemStatus.QueueHandEmbAssignment
                    : ItemStatus.QueueFullStitching,
                ExternalVendorJobStage.FullExternalProduction => ItemStatus.ReadyAtShop,
                _ => item.Status
            };

            var previousStatus = item.Status;
            item.Status = nextStatus;
            job.Status = ExternalVendorJobStatus.Returned;
            job.ReturnedAt = DateTime.UtcNow;
            job.ReturnNotes = notes ?? string.Empty;
            job.ReceivedByWorkerId = receivedByWorkerId;

            AddStatusLog(item, previousStatus, nextStatus, null, notes);
            await _db.SaveChangesAsync();
            return true;
        }

        // --- Core Status Transition & Logging Execution ---

        public async Task<bool> TransitionStatusAsync(
            int orderItemId,
            ItemStatus newStatus,
            int? assignedWorkerId = null,
            string? notes = null,
            int? assignedHandEmbroidererId = null,
            int? branchId = null,
            string? orderId = null)
        {
            var item = branchId.HasValue && !string.IsNullOrWhiteSpace(orderId)
                ? await _db.OrderItems.FirstOrDefaultAsync(i => i.BranchId == branchId.Value && i.OrderId == orderId && i.OrderItemId == orderItemId)
                : await _db.OrderItems.FirstOrDefaultAsync(i => i.OrderItemId == orderItemId);
            if (item == null) return false;

            // Capture prior state and worker assignment
            ItemStatus previousState = item.Status;
            int? previousWorkerId = item.StitchedByWorkerId;

            if (previousState == newStatus)
            {
                return false;
            }

            // Update item state
            item.Status = newStatus;

            if (newStatus == ItemStatus.Delivered)
            {
                item.ActualDeliveryDate = DateTime.UtcNow;
            }

            // Update worker assignment if supplied
            if (assignedWorkerId.HasValue)
            {
                item.StitchedByWorkerId = assignedWorkerId.Value;
            }

            if (assignedHandEmbroidererId.HasValue)
            {
                item.HandEmbroideredByWorkerId = assignedHandEmbroidererId.Value;
            }

            // Create status log entry
            var log = new StatusLog
            {
                BranchId = item.BranchId,
                OrderId = item.OrderId,
                OrderItemId = item.OrderItemId,
                PreviousState = previousState,
                CurrentState = newStatus,
                PreviousWorkerId = previousWorkerId,
                CurrentWorkerId = assignedHandEmbroidererId ?? assignedWorkerId ?? previousWorkerId,
                TimeOfEvent = DateTime.UtcNow,
                Notes = notes
            };

            _db.StatusLogs.Add(log);
            await _db.SaveChangesAsync();

            return true;
        }

        private void AddStatusLog(
            OrderItem item,
            ItemStatus previousState,
            ItemStatus currentState,
            int? currentWorkerId,
            string? notes)
        {
            _db.StatusLogs.Add(new StatusLog
            {
                BranchId = item.BranchId,
                OrderId = item.OrderId,
                OrderItemId = item.OrderItemId,
                PreviousState = previousState,
                CurrentState = currentState,
                PreviousWorkerId = item.ExternalWorkerId,
                CurrentWorkerId = currentWorkerId,
                TimeOfEvent = DateTime.UtcNow,
                Notes = notes
            });
        }
    }
}