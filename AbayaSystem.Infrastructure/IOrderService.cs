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
        Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string query);
        Task<int> GetNextOrderIdForBranchAsync(int branchId);
        Task<ServiceResult> CreateOrderAsync(OrderFormModel model);

        // 🛍️ Fabric Procurement Methods
        Task<List<FabricProcurementItem>> GetPendingAbayaFabricsAsync();
        Task<List<SheilaProcurementItem>> GetPendingSheilaFabricsAsync();
        Task<ServiceResult> MarkFabricAsBoughtAsync(int orderItemId);
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
        public async Task<List<CustomerSearchResultDto>> SearchCustomersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 3)
                return new List<CustomerSearchResultDto>();

            var cleanQuery = query.Trim().ToLower();

            var customers = await _context.Customers
                .Where(c => c.CustomerName.ToLower().Contains(cleanQuery) || c.CustomerPhone.Contains(cleanQuery))
                .Take(10)
                .ToListAsync();

            if (!customers.Any())
                return new List<CustomerSearchResultDto>();

            var customerIds = customers.Select(c => c.CustomerId).ToList();

            var lastOrders = await _context.Orders
                .Where(o => customerIds.Contains(o.CustomerId))
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    LastOrderId = g.OrderByDescending(o => o.OrderDate).Select(o => o.OrderId).FirstOrDefault() ?? ""
                })
                .ToDictionaryAsync(x => x.CustomerId, x => x.LastOrderId);

            return customers.Select(c => new CustomerSearchResultDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerPhone = c.CustomerPhone,
                LastOrderId = lastOrders.TryGetValue(c.CustomerId, out var lastId) ? lastId : string.Empty,
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
    }
}