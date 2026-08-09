using System;
using System.Collections.Generic;
using System.Linq;
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

        // 🛍️ Fabric Procurement Methods
        Task<List<FabricProcurementItem>> GetPendingAbayaFabricsAsync();
        Task<List<SheilaProcurementItem>> GetPendingSheilaFabricsAsync();
        Task<ServiceResult> MarkFabricAsBoughtAsync(int orderItemId);

        // Add to IOrderService interface:
        Task<PagedResult<Order>> GetOrdersPagedAsync(OrderFilterModel filter);

        Task<OrderFormModel?> GetOrderForEditAsync(int branchId, string orderId);
        Task<ServiceResult> UpdateOrderAsync(OrderFormModel model);
    }

    public class OrderService : IOrderService
    {
        private readonly BoutiqueDbContext _context;

        public OrderService(BoutiqueDbContext context)
        {
            _context = context;
        }

        public async Task<List<FabricShop>> GetFabricShopsAsync() =>
            await _context.FabricShops.OrderBy(s => s.FabricShopName).ToListAsync();

        public async Task<List<Fabric>> GetFabricsAsync() =>
            await _context.Fabrics.OrderBy(f => f.FabricName).ToListAsync();

        public async Task<List<Branch>> GetBranchesAsync() =>
            await _context.Branches.ToListAsync();

        public async Task<List<ExternalWorker>> GetExternalWorkersAsync() =>
            await _context.ExternalWorkers.Where(w => w.IsActive).ToListAsync();

        // 🔢 Calculate Next Order Number for Branch
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

        // 🔍 Live Customer Search Method (Returns Last OrderNo)
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
                .ToListAsync(cancellationToken); // 👈 Pass cancellation token here

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
                .ToListAsync(cancellationToken); // 👈 Pass cancellation token here

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

            // 👤 Handle Customer Persistence or Update
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

            // Sync Customer Details and Measurements
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
                TypeOfOrder = OrderType.Internal,
                EstimatedDeliveryDate = model.EstimatedDeliveryDate,
                IsUrgent = model.IsUrgent,
                Notes = model.OrderNotes,
                TotalAmount = model.TotalAmount,
                DepositPaid = model.DepositPaid,
                BalanceDue = model.BalanceDue
            };

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

                var orderItem = new OrderItem
                {
                    BranchId = model.BranchId,
                    OrderId = cleanId,
                    Category = item.Category,
                    ModelTextDescription = item.ModelTextDescription,
                    FabricShopId = item.FabricShopId,
                    FabricId = item.FabricId,
                    ColorCode = item.ColorCode,
                    SelectedSheilaSize = item.SelectedSheilaSize,
                    IsReadyMadeAlteration = item.IsReadyMadeAlteration,
                    AlterationNotes = item.AlterationNotes,
                    Notes = item.ItemNotes,
                    HybridProcess = itemOrderType == OrderType.Hybrid ? item.HybridProcess : HybridProcessType.None,
                    ExternalWorkerId = workerId,
                    BuyFabricForExternal = item.BuyFabricForExternal,
                    TargetBranchId = item.TargetBranchId,
                    Status = (itemOrderType == OrderType.External && !item.BuyFabricForExternal)
                              ? ItemStatus.Completed
                              : ItemStatus.ReadyForFabricProcurement
                };

                order.Items.Add(orderItem);
            }

            _context.Orders.Add(order);
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

            item.Status = ItemStatus.AssignedToCutter;
            await _context.SaveChangesAsync();

            return ServiceResult.Success();
        }

        // Add implementation inside OrderService class:
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
                    .ThenInclude(i => i.ExternalWorker) // Include External Worker info
                .AsQueryable();

            // 🏬 Branch Filter
            if (filter.BranchId.HasValue && filter.BranchId.Value > 0)
            {
                query = query.Where(o => o.BranchId == filter.BranchId.Value);
            }

            // 🔍 Filters
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

            // 🚨 Sorting: Urgent Orders ALWAYS on top first
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
                string workflowKey = "Internal";
                if (item.ExternalWorkerId.HasValue)
                {
                    if (item.HybridProcess != HybridProcessType.None || item.ExternalWorker?.SupportedType == ExternalWorkerType.Hybrid)
                    {
                        workflowKey = $"Hybrid_{item.ExternalWorkerId}";
                    }
                    else
                    {
                        workflowKey = $"External_{item.ExternalWorkerId}";
                    }
                }

                // Lock item if it has already progressed past initial status
                bool isLocked = item.Status != ItemStatus.ReadyForFabricProcurement;

                model.Items.Add(new OrderItemFormModel
                {
                    OrderItemId = item.OrderItemId,
                    Category = item.Category,
                    ModelTextDescription = item.ModelTextDescription,
                    FabricShopId = item.FabricShopId,
                    FabricId = item.FabricId,
                    ColorCode = item.ColorCode,
                    SelectedWorkflowKey = workflowKey,
                    HybridProcess = item.HybridProcess,
                    BuyFabricForExternal = item.BuyFabricForExternal,
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

            // 1. Fetch Existing Order
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.BranchId == model.OriginalBranchId && o.OrderId == model.OriginalOrderId);

            if (order == null)
                return ServiceResult.Failure("Original order not found.");

            // 2. Check if Order Ticket or Branch changed, check collision
            bool isKeyChanged = (model.BranchId != model.OriginalBranchId) || (cleanId != model.OriginalOrderId);
            if (isKeyChanged)
            {
                var exists = await _context.Orders
                    .AnyAsync(o => o.BranchId == model.BranchId && o.OrderId == cleanId);
                if (exists)
                    return ServiceResult.Failure($"Order ticket '{cleanId}' already exists for this branch.");
            }

            // 3. Customer Sync / Persistence
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

            // 4. Update Order Master Record
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

            // 5. Line Items Sync
            var formItemIds = model.Items.Where(i => i.OrderItemId > 0).Select(i => i.OrderItemId).ToList();

            // Prevent removing items already in workflow
            var itemsToRemove = order.Items.Where(i => !formItemIds.Contains(i.OrderItemId)).ToList();
            foreach (var itemToRemove in itemsToRemove)
            {
                if (itemToRemove.Status != ItemStatus.ReadyForFabricProcurement)
                {
                    return ServiceResult.Failure($"Cannot remove item '{itemToRemove.ModelTextDescription}' because it is already in workflow ({itemToRemove.Status}).");
                }
                _context.OrderItems.Remove(itemToRemove);
            }

            // Update existing items or add new items
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

                if (itemModel.OrderItemId > 0)
                {
                    var existingItem = order.Items.FirstOrDefault(i => i.OrderItemId == itemModel.OrderItemId);
                    if (existingItem != null)
                    {
                        if (existingItem.Status == ItemStatus.ReadyForFabricProcurement)
                        {
                            existingItem.Category = itemModel.Category;
                            existingItem.ModelTextDescription = itemModel.ModelTextDescription;
                            existingItem.FabricShopId = itemModel.FabricShopId;
                            existingItem.FabricId = itemModel.FabricId;
                            existingItem.ColorCode = itemModel.ColorCode;
                            existingItem.SelectedSheilaSize = itemModel.SelectedSheilaSize;
                            existingItem.IsReadyMadeAlteration = itemModel.IsReadyMadeAlteration;
                            existingItem.AlterationNotes = itemModel.AlterationNotes;
                            existingItem.Notes = itemModel.ItemNotes;
                            existingItem.HybridProcess = itemOrderType == OrderType.Hybrid ? itemModel.HybridProcess : HybridProcessType.None;
                            existingItem.ExternalWorkerId = workerId;
                            existingItem.BuyFabricForExternal = itemModel.BuyFabricForExternal;
                            existingItem.TargetBranchId = itemModel.TargetBranchId;
                            existingItem.Status = (itemOrderType == OrderType.External && !itemModel.BuyFabricForExternal)
                                ? ItemStatus.Completed
                                : ItemStatus.ReadyForFabricProcurement;
                        }
                        else
                        {
                            // For locked items in workflow, only allow non-workflow descriptions/notes updates
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
                        Category = itemModel.Category,
                        ModelTextDescription = itemModel.ModelTextDescription,
                        FabricShopId = itemModel.FabricShopId,
                        FabricId = itemModel.FabricId,
                        ColorCode = itemModel.ColorCode,
                        SelectedSheilaSize = itemModel.SelectedSheilaSize,
                        IsReadyMadeAlteration = itemModel.IsReadyMadeAlteration,
                        AlterationNotes = itemModel.AlterationNotes,
                        Notes = itemModel.ItemNotes,
                        HybridProcess = itemOrderType == OrderType.Hybrid ? itemModel.HybridProcess : HybridProcessType.None,
                        ExternalWorkerId = workerId,
                        BuyFabricForExternal = itemModel.BuyFabricForExternal,
                        TargetBranchId = itemModel.TargetBranchId,
                        Status = (itemOrderType == OrderType.External && !itemModel.BuyFabricForExternal)
                                  ? ItemStatus.Completed
                                  : ItemStatus.ReadyForFabricProcurement
                    };
                    order.Items.Add(newOrderItem);
                }
            }

            await _context.SaveChangesAsync();
            return ServiceResult.Success();
        }
    }
}