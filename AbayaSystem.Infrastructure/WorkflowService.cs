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

        Task<bool> TransitionStatusAsync(
            int orderItemId,
            ItemStatus newStatus,
            int? assignedWorkerId = null,
            string? notes = null,
            int? assignedHandEmbroidererId = null);
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
            if (item.TypeOfOrder == OrderType.External)
            {
                return ItemStatus.QueueExternalVendor;
            }

            if (item.TypeOfOrder == OrderType.Hybrid && item.rawFabricEmb)
            {
                return ItemStatus.QueueRawFabricEmb;
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
            if (orderType == OrderType.Hybrid && !rawFabricEmb)
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

        // --- Core Status Transition & Logging Execution ---

        public async Task<bool> TransitionStatusAsync(
            int orderItemId,
            ItemStatus newStatus,
            int? assignedWorkerId = null,
            string? notes = null,
            int? assignedHandEmbroidererId = null)
        {
            var item = await _db.OrderItems.FindAsync(orderItemId);
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
    }
}