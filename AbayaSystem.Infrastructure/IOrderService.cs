using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AbayaSystem.Core;

namespace AbayaSystem.Infrastructure
{
    public interface IOrderService
    {
        Task<List<FabricShop>> GetFabricShopsAsync();
        Task<List<Fabric>> GetFabricsAsync();
        Task<List<Branch>> GetBranchesAsync();
        Task<List<ExternalWorker>> GetExternalWorkersAsync();
        Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string query, CancellationToken cancellationToken = default);
        Task<int> GetNextOrderIdForBranchAsync(int branchId);
        Task<ServiceResult> CreateOrderAsync(OrderFormModel model);
        Task<List<FabricProcurementItem>> GetPendingAbayaFabricsAsync();
        Task<List<SheilaProcurementItem>> GetPendingSheilaFabricsAsync();
        Task<ServiceResult> MarkFabricAsBoughtAsync(int orderItemId);
        Task<PagedResult<Order>> GetOrdersPagedAsync(OrderFilterModel filter);
        Task<DashboardSummary> GetDashboardSummaryAsync(int? branchId = null, DateTime? orderDateFrom = null, DateTime? orderDateTo = null);
        Task<OrderFormModel?> GetOrderForEditAsync(int branchId, string orderId);
        Task<ServiceResult> UpdateOrderAsync(OrderFormModel model);
    }

    public class OrderService : IOrderService
    {
        private readonly BoutiqueDbContext _context;
        private readonly IWorkflowService _workflowService;

        public OrderService(BoutiqueDbContext context, IWorkflowService workflowService)
        {
            _context = context;
            _workflowService = workflowService;
        }

        public async Task<List<FabricShop>> GetFabricShopsAsync() =>
            await _context.FabricShops.OrderBy(s => s.FabricShopName).ToListAsync();

        public async Task<List<Fabric>> GetFabricsAsync() =>
            await _context.Fabrics.OrderBy(f => f.FabricName).ToListAsync();

        public async Task<List<Branch>> GetBranchesAsync() =>
            await _context.Branches.ToListAsync();

        public async Task<List<ExternalWorker>> GetExternalWorkersAsync() =>
            await _context.ExternalWorkers.Where(w => w.IsActive).ToListAsync();

        public async Task<int> GetNextOrderIdForBranchAsync(int branchId)
        {
            var orderIds = await _context.Orders
                .Where(o => o.BranchId == branchId)
                .Select(o => o.OrderId)
                .ToListAsync();

            int maxOrderNo = 0;
            foreach (var id in orderIds)
            {
                if (int.TryParse(id, out int parsed))
                {
                    if (parsed > maxOrderNo) maxOrderNo = parsed;
                }
            }

            return maxOrderNo + 1;
        }

        public async Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string query, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 3)
                return new List<CustomerSearchResultDto>();

            var cleanQuery = query.Trim().ToLower();

            var customers = await _context.Customers
                .AsNoTracking()
                .Where(c => c.CustomerName.ToLower().Contains(cleanQuery) ||
                            c.CustomerPhone.Contains(cleanQuery))
                .Take(10)
                .ToListAsync(cancellationToken);

            if (!customers.Any())
                return new List<CustomerSearchResultDto>();

            var customerIds = customers.Select(c => c.CustomerId).ToList();

            var lastOrders = await _context.Orders
                .AsNoTracking()
                .Where(o => customerIds.Contains(o.CustomerId))
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    LastOrderId = g.OrderByDescending(o => o.OrderDate).Select(o => o.OrderId).FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            return customers.Select(c => new CustomerSearchResultDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerPhone = c.CustomerPhone,
                LastOrderId = lastOrders.FirstOrDefault(o => o.CustomerId == c.CustomerId)?.LastOrderId ?? string.Empty,
                LengthAbayaFront = c.LengthAbayaFront,
                LengthAbayaBack = c.LengthAbayaBack,
                LengthSleeve = c.LengthSleeve,
                WidthArmHole = c.WidthArmHole,
                WidthSleeveOpening = c.WidthSleeveOpening,
                WidthShoulder = c.WidthShoulder,
                WidthBody = c.WidthBody,
                WidthBottom = c.WidthBottom,
                ButtonType = c.ButtonType,
                NumberOfButtons = c.NumberOfButtons
            }).ToList();
        }

        public async Task<ServiceResult> CreateOrderAsync(OrderFormModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ManualOrderId))
                return ServiceResult.Failure("Specify a valid manual ticket number!");

            if (model.Items.Count == 0)
                return ServiceResult.Failure("You must add at least one item to the order!");

            if (string.IsNullOrWhiteSpace(model.CustomerName) || string.IsNullOrWhiteSpace(model.CustomerPhone))
                return ServiceResult.Failure("Customer name and phone number are required!");

            var cleanId = model.ManualOrderId.Trim().ToUpper();

            var exist = await _context.Orders
                .AnyAsync(o => o.BranchId == model.BranchId && o.OrderId == cleanId);

            if (exist)
                return ServiceResult.Failure($"Order ticket '{cleanId}' already exists for this branch.");

            Customer customer;
            if (model.CustomerId.HasValue && model.CustomerId.Value > 0)
            {
                customer = await _context.Customers.FindAsync(model.CustomerId.Value);
                if (customer == null)
                {
                    return ServiceResult.Failure("Selected customer profile was not found.");
                }
            }
            else
            {
                var cleanPhone = model.CustomerPhone.Trim();
                customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerPhone == cleanPhone);
                if (customer == null)
                {
                    customer = new Customer();
                    _context.Customers.Add(customer);
                }
            }

            customer.CustomerName = model.CustomerName.Trim();
            customer.CustomerPhone = model.CustomerPhone.Trim();
            customer.LengthAbayaFront = model.LengthAbayaFront;
            customer.LengthAbayaBack = model.LengthAbayaBack;
            customer.LengthSleeve = model.LengthSleeve;
            customer.WidthArmHole = model.WidthArmHole;
            customer.WidthSleeveOpening = model.WidthSleeveOpening;
            customer.WidthShoulder = model.WidthShoulder;
            customer.WidthBody = model.WidthBody;
            customer.WidthBottom = model.WidthBottom;
            customer.ButtonType = model.ButtonType;
            customer.NumberOfButtons = model.NumberOfButtons;

            await _context.SaveChangesAsync();

            var order = new Order
            {
                BranchId = model.BranchId,
                OrderId = cleanId,
                CustomerId = customer.CustomerId,
                OrderDate = model.OrderDate,
                EstimatedDeliveryDate = model.EstimatedDeliveryDate,
                IsUrgent = model.IsUrgent,
                Notes = model.OrderNotes,
                TotalAmount = model.TotalAmount,
                DepositPaid = model.DepositPaid,
                BalanceDue = model.BalanceDue
            };

            var nextOrderItemId = 1;
            foreach (var item in model.Items)
            {
                OrderType itemOrderType = OrderType.Internal;
                int? workerId = null;

                if (item.SelectedWorkflowKey.StartsWith("Hybrid_"))
                {
                    itemOrderType = OrderType.Hybrid;
                    workerId = int.Parse(item.SelectedWorkflowKey.Replace("Hybrid_", ""));
                }
                else if (item.SelectedWorkflowKey.StartsWith("External_"))
                {
                    itemOrderType = OrderType.External;
                    workerId = int.Parse(item.SelectedWorkflowKey.Replace("External_", ""));
                }

                bool isHybrid = itemOrderType == OrderType.Hybrid;

                ItemStatus initialStatus = _workflowService.DetermineInitialStatus(
                    itemOrderType,
                    item.BuyFabricForExternal);

                var orderItem = new OrderItem
                {
                    BranchId = model.BranchId,
                    OrderId = cleanId,
                    OrderItemId = nextOrderItemId++,
                    Category = item.Category,
                    ModelTextDescription = item.ModelTextDescription,
                    FabricShopId = item.FabricShopId,
                    FabricId = item.FabricId,
                    ColorCode = item.ColorCode,
                    SelectedSheilaSize = item.SelectedSheilaSize,
                    //IsSheilaFabricBought=(item.SelectedSheilaSize==SheilaSize.Size_28x90)?false:true,
                    IsReadyMadeAlteration = item.IsReadyMadeAlteration,
                    AlterationNotes = item.AlterationNotes,
                    Notes = item.ItemNotes,
                    ExternalWorkerId = workerId,
                    BuyFabricForExternal = item.BuyFabricForExternal,
                    HandEmbRequired = item.HandEmbRequired,
                    rawFabricEmb = isHybrid && item.rawFabricEmb,
                    TargetBranchId = item.TargetBranchId,
                    Status = initialStatus
                };

                order.Items.Add(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var orderItem in order.Items)
            {
                _context.StatusLogs.Add(new AbayaSystem.Core.AbayaSystem.Core.StatusLog
                {
                    OrderId = orderItem.OrderId,
                    OrderItemId = orderItem.OrderItemId,
                    PreviousState = null,
                    CurrentState = orderItem.Status,
                    CurrentWorkerId = orderItem.StitchedByWorkerId,
                    TimeOfEvent = DateTime.UtcNow,
                    Notes = "Initial workflow status assigned when the order was created."
                });
            }

            await _context.SaveChangesAsync();

            return ServiceResult.Success();
        }

        public async Task<List<FabricProcurementItem>> GetPendingAbayaFabricsAsync()
        {
            return await _context.OrderItems
                .Include(i => i.Order)
                .Include(i => i.FabricShop)
                .Include(i => i.Fabric)
                .Where(i => i.Status == ItemStatus.ReadyForFabricProcurement && (i.FabricShopId != null || i.FabricId != null))
                .Select(i => new FabricProcurementItem
                {
                    OrderItemId = i.OrderItemId,
                    BranchId = i.BranchId,
                    OrderId = i.OrderId,
                    ModelDescription = i.ModelTextDescription,
                    FabricShopName = i.FabricShop != null ? i.FabricShop.FabricShopName : "N/A",
                    FabricName = i.Fabric != null ? i.Fabric.FabricName : "N/A",
                    ColorCode = i.ColorCode,
                    OrderDate = i.Order.OrderDate
                })
                .ToListAsync();
        }

        public async Task<List<SheilaProcurementItem>> GetPendingSheilaFabricsAsync()
        {
            return await _context.OrderItems
                .Include(i => i.Order)
                .Include(i => i.FabricShop)
                .Include(i => i.Fabric)
                .Where(i => i.Status == ItemStatus.ReadyForFabricProcurement && i.SelectedSheilaSize == SheilaSize.Size_28x90)
                .Select(i => new SheilaProcurementItem
                {
                    OrderItemId = i.OrderItemId,
                    BranchId = i.BranchId,
                    OrderId = i.OrderId,
                    ModelTextDescription = i.ModelTextDescription,
                    SelectedSheilaSize = i.SelectedSheilaSize,
                    FabricShopName = i.FabricShop != null ? i.FabricShop.FabricShopName : "N/A",
                    FabricName = i.Fabric != null ? i.Fabric.FabricName : "N/A",
                    ColorCode = i.ColorCode,
                    OrderDate = i.Order.OrderDate
                })
                .ToListAsync();
        }

        public async Task<ServiceResult> MarkFabricAsBoughtAsync(int orderItemId)
        {
            var item = await _context.OrderItems.FindAsync(orderItemId);
            if (item == null) return ServiceResult.Failure("Order item not found.");

            var nextStatus = _workflowService.DetermineNextStatusAfterFabricProcurement(item);
            await _workflowService.TransitionStatusAsync(
                item.OrderItemId,
                nextStatus,
                notes: "Fabric procurement completed.");

            return ServiceResult.Success();
        }

        public async Task<PagedResult<Order>> GetOrdersPagedAsync(OrderFilterModel filter)
        {
            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Branch)
                .Include(o => o.Items)
                    .ThenInclude(i => i.FabricShop)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Fabric)
                .Include(o => o.Items)
                    .ThenInclude(i => i.ExternalWorker)
                .AsQueryable();

            if (filter.BranchId.HasValue && filter.BranchId.Value > 0)
            {
                query = query.Where(o => o.BranchId == filter.BranchId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.OrderId))
            {
                var cleanId = filter.OrderId.Trim().ToLower();
                query = query.Where(o => o.OrderId.ToLower().Contains(cleanId));
            }

            if (filter.OrderDateFrom.HasValue)
                query = query.Where(o => o.OrderDate >= filter.OrderDateFrom.Value.Date);

            if (filter.OrderDateTo.HasValue)
                query = query.Where(o => o.OrderDate <= filter.OrderDateTo.Value.Date.AddDays(1).AddTicks(-1));

            if (filter.DeliveryDateFrom.HasValue)
                query = query.Where(o => o.EstimatedDeliveryDate >= filter.DeliveryDateFrom.Value.Date);

            if (filter.DeliveryDateTo.HasValue)
                query = query.Where(o => o.EstimatedDeliveryDate <= filter.DeliveryDateTo.Value.Date.AddDays(1).AddTicks(-1));

            if (!string.IsNullOrWhiteSpace(filter.CustomerName))
            {
                var cleanName = filter.CustomerName.Trim().ToLower();
                query = query.Where(o => o.Customer.CustomerName.ToLower().Contains(cleanName));
            }

            if (!string.IsNullOrWhiteSpace(filter.CustomerPhone))
            {
                var cleanPhone = filter.CustomerPhone.Trim();
                query = query.Where(o => o.Customer.CustomerPhone.Contains(cleanPhone));
            }

            if (filter.ItemStatus.HasValue)
            {
                query = query.Where(o => o.Items.Any(i => i.Status == filter.ItemStatus.Value));
            }

            IOrderedQueryable<Order> orderedQuery;

            if (filter.SortBy == "DeliveryDate")
            {
                orderedQuery = filter.SortDescending
                    ? query.OrderByDescending(o => o.IsUrgent).ThenByDescending(o => o.EstimatedDeliveryDate)
                    : query.OrderByDescending(o => o.IsUrgent).ThenBy(o => o.EstimatedDeliveryDate);
            }
            else
            {
                orderedQuery = filter.SortDescending
                    ? query.OrderByDescending(o => o.IsUrgent).ThenByDescending(o => o.OrderDate)
                    : query.OrderByDescending(o => o.IsUrgent).ThenBy(o => o.OrderDate);
            }

            var totalCount = await orderedQuery.CountAsync();

            var items = await orderedQuery
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Order>
            {
                Items = items,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<DashboardSummary> GetDashboardSummaryAsync(int? branchId = null, DateTime? orderDateFrom = null, DateTime? orderDateTo = null)
        {
            var ordersQuery = _context.Orders
                .AsNoTracking()
                .Include(o => o.Branch)
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .AsQueryable();

            if (branchId.HasValue && branchId.Value > 0)
            {
                ordersQuery = ordersQuery.Where(o => o.BranchId == branchId.Value);
            }

            if (orderDateFrom.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate >= orderDateFrom.Value.Date);
            }

            if (orderDateTo.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate <= orderDateTo.Value.Date.AddDays(1).AddTicks(-1));
            }

            var orders = await ordersQuery.ToListAsync();
            var items = orders.SelectMany(o => o.Items).ToList();

            var undeliveredOrdersQuery = _context.Orders
                .AsNoTracking()
                .Include(o => o.Branch)
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .Where(o => o.Items.Any(i => i.Status != ItemStatus.Delivered));

            if (branchId.HasValue && branchId.Value > 0)
            {
                undeliveredOrdersQuery = undeliveredOrdersQuery.Where(o => o.BranchId == branchId.Value);
            }

            var undeliveredOrders = await undeliveredOrdersQuery.ToListAsync();

            var currentTaskStatuses = new[]
            {
                ItemStatus.QueueHalfStitching,
                ItemStatus.HalfStitchActive,
                ItemStatus.QueueFullStitching,
                ItemStatus.FullStitchActive,
                ItemStatus.QueueHandEmb,
                ItemStatus.HandEmbActive
            };

            var workerTasksQuery = _context.OrderItems
                .AsNoTracking()
                .Include(i => i.Order)
                    .ThenInclude(o => o.Customer)
                .Where(i => currentTaskStatuses.Contains(i.Status) &&
                    (i.StitchedByWorkerId.HasValue || i.HandEmbroideredByWorkerId.HasValue));

            if (branchId.HasValue && branchId.Value > 0)
            {
                workerTasksQuery = workerTasksQuery.Where(i => i.BranchId == branchId.Value);
            }

            var workerTaskItems = await workerTasksQuery.ToListAsync();
            var workerIds = workerTaskItems
                .Select(i => i.Status == ItemStatus.QueueHandEmb || i.Status == ItemStatus.HandEmbActive
                    ? i.HandEmbroideredByWorkerId
                    : i.StitchedByWorkerId)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();
            var workers = await _context.Workers
                .AsNoTracking()
                .Where(w => workerIds.Contains(w.WorkerId))
                .ToDictionaryAsync(w => w.WorkerId);
            var workerTaskItemIds = workerTaskItems.Select(i => i.OrderItemId).ToList();
            var taskLogs = await _context.StatusLogs
                .AsNoTracking()
                .Where(l => workerTaskItemIds.Contains(l.OrderItemId))
                .OrderByDescending(l => l.TimeOfEvent)
                .ToListAsync();
            var today = DateTime.Today;

            return new DashboardSummary
            {
                TotalOrders = orders.Count,
                TotalItems = items.Count,
                ActiveItems = items.Count(i => i.Status != ItemStatus.Delivered),
                DeliveredItems = items.Count(i => i.Status == ItemStatus.Delivered),
                UrgentOrders = orders.Count(o => o.IsUrgent),
                OverdueOrders = orders.Count(o => o.EstimatedDeliveryDate.Date < today && o.Items.Any(i => i.Status != ItemStatus.Delivered)),
                PendingFabricProcurement = items.Count(i => i.Status == ItemStatus.ReadyForFabricProcurement),
                ExternalItemsInProgress = items.Count(i => i.Status == ItemStatus.QueueExternalVendor || i.Status == ItemStatus.OutWithExternalVendor || i.Status == ItemStatus.QueueRawFabricEmb || i.Status == ItemStatus.OutForRawFabricEmb || i.Status == ItemStatus.QueueHalfStitchEmb || i.Status == ItemStatus.OutForHalfStitchEmb),
                TotalAmount = orders.Sum(o => o.TotalAmount),
                DepositsReceived = orders.Sum(o => o.DepositPaid),
                BalanceDue = orders.Sum(o => o.BalanceDue),
                StatusCounts = items
                    .GroupBy(i => i.Status)
                    .Select(g => new DashboardStatusCount { Status = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToList(),
                RecentOrders = orders
                    .OrderByDescending(o => o.OrderDate)
                    .ThenByDescending(o => o.IsUrgent)
                    .Take(8)
                    .Select(o => new DashboardRecentOrder
                    {
                        BranchId = o.BranchId,
                        OrderId = o.OrderId,
                        CustomerName = o.Customer?.CustomerName ?? "Unknown customer",
                        BranchName = o.Branch?.BranchName ?? "Unknown branch",
                        OrderDate = o.OrderDate,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        TotalAmount = o.TotalAmount,
                        IsUrgent = o.IsUrgent,
                        ItemCount = o.Items.Count,
                        PrimaryStatus = o.Items.FirstOrDefault()?.Status
                    })
                    .ToList(),
                UndeliveredItems = undeliveredOrders
                    .SelectMany(o => o.Items.Select(i => new DashboardUndeliveredItem
                    {
                        BranchId = o.BranchId,
                        OrderItemId = i.OrderItemId,
                        OrderId = o.OrderId,
                        CustomerName = o.Customer?.CustomerName ?? "Unknown customer",
                        BranchName = o.Branch?.BranchName ?? "Unknown branch",
                        ModelDescription = i.ModelTextDescription,
                        Status = i.Status,
                        OrderDate = o.OrderDate,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        IsUrgent = o.IsUrgent
                    }))
                    .Where(i => i.Status != ItemStatus.Delivered)
                    .OrderBy(i => i.EstimatedDeliveryDate)
                    .ThenByDescending(i => i.IsUrgent)
                    .ToList(),
                WorkerTasks = workerTaskItems
                    .Select(i =>
                    {
                        var isHandEmbroidery = i.Status == ItemStatus.QueueHandEmb || i.Status == ItemStatus.HandEmbActive;
                        var workerId = isHandEmbroidery ? i.HandEmbroideredByWorkerId : i.StitchedByWorkerId;
                        var worker = workerId.HasValue && workers.TryGetValue(workerId.Value, out var assignedWorker)
                            ? assignedWorker
                            : null;
                        var startLog = taskLogs.FirstOrDefault(l =>
                            l.OrderItemId == i.OrderItemId &&
                            l.CurrentWorkerId == workerId &&
                            l.CurrentState == i.Status)
                            ?? taskLogs.FirstOrDefault(l => l.OrderItemId == i.OrderItemId && l.CurrentState == i.Status);

                        return new DashboardWorkerTask
                        {
                            WorkerId = workerId ?? 0,
                            WorkerName = worker?.Name ?? "Unassigned",
                            WorkerRole = isHandEmbroidery ? "Hand Embroiderer" : "Tailor",
                            OrderItemId = i.OrderItemId,
                            OrderId = i.OrderId,
                            CustomerName = i.Order?.Customer?.CustomerName ?? "Unknown customer",
                            ModelDescription = i.ModelTextDescription,
                            Status = i.Status,
                            TaskStartedAt = startLog?.TimeOfEvent ?? i.Order?.OrderDate ?? DateTime.Today,
                            EstimatedDeliveryDate = i.Order?.EstimatedDeliveryDate ?? DateTime.Today
                        };
                    })
                    .Where(t => t.WorkerId > 0)
                    .OrderBy(t => t.WorkerName)
                    .ThenBy(t => t.TaskStartedAt)
                    .ToList()
            };
        }

        public async Task<OrderFormModel?> GetOrderForEditAsync(int branchId, string orderId)
        {
            var cleanId = orderId.Trim().ToUpper();
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(i => i.ExternalWorker)
                .FirstOrDefaultAsync(o => o.BranchId == branchId && o.OrderId == cleanId);

            if (order == null) return null;

            var model = new OrderFormModel
            {
                IsEditMode = true,
                OriginalBranchId = order.BranchId,
                OriginalOrderId = order.OrderId,
                BranchId = order.BranchId,
                ManualOrderId = order.OrderId,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.CustomerName ?? string.Empty,
                CustomerPhone = order.Customer?.CustomerPhone ?? string.Empty,
                LengthAbayaFront = order.Customer?.LengthAbayaFront ?? 0,
                LengthAbayaBack = order.Customer?.LengthAbayaBack ?? 0,
                LengthSleeve = order.Customer?.LengthSleeve ?? 0,
                WidthArmHole = order.Customer?.WidthArmHole ?? 0,
                WidthSleeveOpening = order.Customer?.WidthSleeveOpening ?? 0,
                WidthShoulder = order.Customer?.WidthShoulder ?? 0,
                WidthBody = order.Customer?.WidthBody ?? 0,
                WidthBottom = order.Customer?.WidthBottom ?? 0,
                ButtonType = order.Customer?.ButtonType ?? ButtonType.NoButtons,
                NumberOfButtons = order.Customer?.NumberOfButtons ?? 0,
                OrderDate = order.OrderDate,
                EstimatedDeliveryDate = order.EstimatedDeliveryDate,
                IsUrgent = order.IsUrgent,
                OrderNotes = order.Notes,
                TotalAmount = order.TotalAmount,
                DepositPaid = order.DepositPaid
            };

            foreach (var item in order.Items)
            {
                string workflowKey = item.TypeOfOrder switch
                {
                    OrderType.Hybrid when item.ExternalWorkerId.HasValue => $"Hybrid_{item.ExternalWorkerId}",
                    OrderType.External when item.ExternalWorkerId.HasValue => $"External_{item.ExternalWorkerId}",
                    _ => "Internal"
                };

                // Item is considered locked if it has progressed past its initial queue state
                bool isLocked = item.Status != ItemStatus.ReadyForFabricProcurement && item.Status != ItemStatus.QueueExternalVendor;

                model.Items.Add(new OrderItemFormModel
                {
                    OrderItemId = item.OrderItemId,
                    Category = item.Category,
                    ModelTextDescription = item.ModelTextDescription,
                    FabricShopId = item.FabricShopId,
                    FabricId = item.FabricId,
                    ColorCode = item.ColorCode,
                    SelectedWorkflowKey = workflowKey,
                    BuyFabricForExternal = item.BuyFabricForExternal,
                    HandEmbRequired = item.HandEmbRequired,
                    rawFabricEmb = item.rawFabricEmb,
                    SelectedSheilaSize = item.SelectedSheilaSize,
                    IsReadyMadeAlteration = item.IsReadyMadeAlteration,
                    AlterationNotes = item.AlterationNotes,
                    ItemNotes = item.Notes,
                    TargetBranchId = item.TargetBranchId,
                    Status = item.Status,
                    IsLocked = isLocked
                });
            }

            return model;
        }

        public async Task<ServiceResult> UpdateOrderAsync(OrderFormModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ManualOrderId))
                return ServiceResult.Failure("Specify a valid manual ticket number!");

            if (model.Items.Count == 0)
                return ServiceResult.Failure("You must have at least one item in the order!");

            if (string.IsNullOrWhiteSpace(model.CustomerName) || string.IsNullOrWhiteSpace(model.CustomerPhone))
                return ServiceResult.Failure("Customer name and phone number are required!");

            var cleanId = model.ManualOrderId.Trim().ToUpper();

            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.BranchId == model.OriginalBranchId && o.OrderId == model.OriginalOrderId);

            if (order == null)
                return ServiceResult.Failure("Original order not found.");

            bool isKeyChanged = (model.BranchId != model.OriginalBranchId) || (cleanId != model.OriginalOrderId);
            if (isKeyChanged)
            {
                var exists = await _context.Orders
                    .AnyAsync(o => o.BranchId == model.BranchId && o.OrderId == cleanId);
                if (exists)
                    return ServiceResult.Failure($"Order ticket '{cleanId}' already exists for this branch.");
            }

            Customer customer;
            if (model.CustomerId.HasValue && model.CustomerId.Value > 0)
            {
                customer = await _context.Customers.FindAsync(model.CustomerId.Value);
                if (customer == null) return ServiceResult.Failure("Selected customer profile was not found.");
            }
            else
            {
                var cleanPhone = model.CustomerPhone.Trim();
                customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerPhone == cleanPhone);
                if (customer == null)
                {
                    customer = new Customer();
                    _context.Customers.Add(customer);
                }
            }

            customer.CustomerName = model.CustomerName.Trim();
            customer.CustomerPhone = model.CustomerPhone.Trim();
            customer.LengthAbayaFront = model.LengthAbayaFront;
            customer.LengthAbayaBack = model.LengthAbayaBack;
            customer.LengthSleeve = model.LengthSleeve;
            customer.WidthArmHole = model.WidthArmHole;
            customer.WidthSleeveOpening = model.WidthSleeveOpening;
            customer.WidthShoulder = model.WidthShoulder;
            customer.WidthBody = model.WidthBody;
            customer.WidthBottom = model.WidthBottom;
            customer.ButtonType = model.ButtonType;
            customer.NumberOfButtons = model.NumberOfButtons;

            await _context.SaveChangesAsync();

            order.CustomerId = customer.CustomerId;
            order.OrderDate = model.OrderDate;
            order.EstimatedDeliveryDate = model.EstimatedDeliveryDate;
            order.IsUrgent = model.IsUrgent;
            order.Notes = model.OrderNotes;
            order.TotalAmount = model.TotalAmount;
            order.DepositPaid = model.DepositPaid;
            order.BalanceDue = model.BalanceDue;

            if (isKeyChanged)
            {
                order.BranchId = model.BranchId;
                order.OrderId = cleanId;
                foreach (var item in order.Items)
                {
                    item.BranchId = model.BranchId;
                    item.OrderId = cleanId;
                }
            }

            var formItemIds = model.Items.Where(i => i.OrderItemId > 0).Select(i => i.OrderItemId).ToList();
            var nextOrderItemId = order.Items.Any() ? order.Items.Max(i => i.OrderItemId) + 1 : 1;
            var statusEvents = new List<(OrderItem Item, ItemStatus? PreviousState, ItemStatus CurrentState)>();

            var itemsToRemove = order.Items.Where(i => !formItemIds.Contains(i.OrderItemId)).ToList();
            foreach (var itemToRemove in itemsToRemove)
            {
                if (itemToRemove.Status != ItemStatus.ReadyForFabricProcurement && itemToRemove.Status != ItemStatus.QueueExternalVendor)
                {
                    return ServiceResult.Failure($"Cannot remove item '{itemToRemove.ModelTextDescription}' because it is already in active workflow ({itemToRemove.Status}).");
                }
                _context.OrderItems.Remove(itemToRemove);
            }

            foreach (var itemModel in model.Items)
            {
                OrderType itemOrderType = OrderType.Internal;
                int? workerId = null;

                if (itemModel.SelectedWorkflowKey.StartsWith("Hybrid_"))
                {
                    itemOrderType = OrderType.Hybrid;
                    workerId = int.Parse(itemModel.SelectedWorkflowKey.Replace("Hybrid_", ""));
                }
                else if (itemModel.SelectedWorkflowKey.StartsWith("External_"))
                {
                    itemOrderType = OrderType.External;
                    workerId = int.Parse(itemModel.SelectedWorkflowKey.Replace("External_", ""));
                }

                bool isHybrid = itemOrderType == OrderType.Hybrid;

                ItemStatus initialStatus = _workflowService.DetermineInitialStatus(
                    itemOrderType,
                    itemModel.BuyFabricForExternal);

                if (itemModel.OrderItemId > 0)
                {
                    var existingItem = order.Items.FirstOrDefault(i => i.OrderItemId == itemModel.OrderItemId);
                    if (existingItem != null)
                    {
                        if (existingItem.Status == ItemStatus.ReadyForFabricProcurement || existingItem.Status == ItemStatus.QueueExternalVendor)
                        {
                            existingItem.Category = itemModel.Category;
                            existingItem.ModelTextDescription = itemModel.ModelTextDescription;
                            existingItem.FabricShopId = itemModel.FabricShopId;
                            existingItem.FabricId = itemModel.FabricId;
                            existingItem.ColorCode = itemModel.ColorCode;
                            existingItem.SelectedSheilaSize = itemModel.SelectedSheilaSize;
                            //existingItem.IsSheilaFabricBought = (itemModel.SelectedSheilaSize == SheilaSize.Size_28x90) ? false : true;

                            existingItem.IsReadyMadeAlteration = itemModel.IsReadyMadeAlteration;
                            existingItem.AlterationNotes = itemModel.AlterationNotes;
                            existingItem.Notes = itemModel.ItemNotes;
                            existingItem.ExternalWorkerId = workerId;
                            existingItem.BuyFabricForExternal = itemModel.BuyFabricForExternal;
                            existingItem.HandEmbRequired = itemModel.HandEmbRequired;
                            existingItem.rawFabricEmb = isHybrid && itemModel.rawFabricEmb;
                            existingItem.TargetBranchId = itemModel.TargetBranchId;
                            if (existingItem.Status != initialStatus)
                            {
                                statusEvents.Add((existingItem, existingItem.Status, initialStatus));
                                existingItem.Status = initialStatus;
                            }
                        }
                        else
                        {
                            existingItem.ModelTextDescription = itemModel.ModelTextDescription;
                            existingItem.Notes = itemModel.ItemNotes;
                            existingItem.AlterationNotes = itemModel.AlterationNotes;
                        }
                    }
                }
                else
                {
                    var newOrderItem = new OrderItem
                    {
                        BranchId = model.BranchId,
                        OrderId = cleanId,
                        OrderItemId = nextOrderItemId++,
                        Category = itemModel.Category,
                        ModelTextDescription = itemModel.ModelTextDescription,
                        FabricShopId = itemModel.FabricShopId,
                        FabricId = itemModel.FabricId,
                        ColorCode = itemModel.ColorCode,
                        SelectedSheilaSize = itemModel.SelectedSheilaSize,
                        IsReadyMadeAlteration = itemModel.IsReadyMadeAlteration,
                        AlterationNotes = itemModel.AlterationNotes,
                        Notes = itemModel.ItemNotes,
                        ExternalWorkerId = workerId,
                        BuyFabricForExternal = itemModel.BuyFabricForExternal,
                        HandEmbRequired = itemModel.HandEmbRequired,
                        rawFabricEmb = isHybrid && itemModel.rawFabricEmb,
                        TargetBranchId = itemModel.TargetBranchId,
                        Status = initialStatus
                    };
                    order.Items.Add(newOrderItem);
                    statusEvents.Add((newOrderItem, null, initialStatus));
                }
            }

            await _context.SaveChangesAsync();

            foreach (var statusEvent in statusEvents)
            {
                _context.StatusLogs.Add(new AbayaSystem.Core.AbayaSystem.Core.StatusLog
                {
                    OrderId = statusEvent.Item.OrderId,
                    OrderItemId = statusEvent.Item.OrderItemId,
                    PreviousState = statusEvent.PreviousState,
                    CurrentState = statusEvent.CurrentState,
                    PreviousWorkerId = statusEvent.Item.StitchedByWorkerId,
                    CurrentWorkerId = statusEvent.Item.StitchedByWorkerId,
                    TimeOfEvent = DateTime.UtcNow,
                    Notes = "Workflow status assigned while the order was updated."
                });
            }

            await _context.SaveChangesAsync();
            return ServiceResult.Success();
        }
    }
}