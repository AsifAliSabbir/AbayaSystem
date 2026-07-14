using Microsoft.EntityFrameworkCore;
using AbayaSystem.Core;

namespace AbayaSystem.Infrastructure
{
    public interface IOrderService
    {
        Task<ServiceResult> CreateManualOrderAsync(OrderFormModel model);

        Task<List<FabricProcurementItem>> GetPendingAbayaFabricsAsync();
        Task<List<SheilaProcurementItem>> GetPendingSheilaFabricsAsync();
        Task<bool> MarkFabricAsBoughtAsync(int orderItemId, bool isSheila);
    }

    public class OrderService : IOrderService
    {
        private readonly BoutiqueDbContext _context;

        public OrderService(BoutiqueDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult> CreateManualOrderAsync(OrderFormModel model)
        {
            // 1. Check for empty inputs
            if (string.IsNullOrWhiteSpace(model.ManualOrderId))
            {
                return ServiceResult.Failure("You must specify a valid, manual order/ticket number!");
            }

            var cleanId = model.ManualOrderId.Trim().ToUpper();

            // 2. Validate against duplicate manual primary keys in the database
            var exist = await _context.Orders.AnyAsync(o => o.OrderId == cleanId);
            if (exist)
            {
                return ServiceResult.Failure($"An order with number '{cleanId}' already exists in your records.");
            }

            // 3. Map out the parent Order record
            var order = new Order
            {
                OrderId = cleanId,
                CustomerName = model.TypeOfOrder == OrderType.InternalStockReplenishment ? "Shop Display Rack" : model.CustomerName,
                TypeOfOrder = model.TypeOfOrder,
                TotalAmount = model.TypeOfOrder == OrderType.InternalStockReplenishment ? 0 : model.TotalAmount,
                DepositPaid = model.TypeOfOrder == OrderType.InternalStockReplenishment ? 0 : model.DepositPaid
            };

            // 4. Map the line item description details
            var orderItem = new OrderItem
            {
                OrderId = cleanId,
                ModelTextDescription = model.ModelTextDescription,
                FabricName = model.FabricName,
                IsShopProvidingFabric = model.IsShopProvidingFabric,
                SelectedSheilaSize = model.SelectedSheilaSize,
                IsReadyMadeAlteration = model.IsReadyMadeAlteration,
                AlterationNotes = model.IsReadyMadeAlteration ? model.AlterationNotes : string.Empty,
                Status = model.IsReadyMadeAlteration ? ItemStatus.AlterationActive : ItemStatus.ReadyForFabricProcurement
            };

            order.Items.Add(orderItem);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return ServiceResult.Success();
        }

            public async Task<List<FabricProcurementItem>> GetPendingAbayaFabricsAsync()
        {
            return await _context.OrderItems
                .Where(i => i.IsShopProvidingFabric && !i.IsAbayaFabricBought && i.Status == ItemStatus.ReadyForFabricProcurement)
                .Select(i => new FabricProcurementItem
                {
                    OrderItemId = i.OrderItemId,
                    OrderId = i.OrderId,
                    FabricName = i.FabricName,
                    ModelDescription = i.ModelTextDescription
                })
                .ToListAsync();
        }

        public async Task<List<SheilaProcurementItem>> GetPendingSheilaFabricsAsync()
        {
            return await _context.OrderItems
                .Where(i => i.SelectedSheilaSize == SheilaSize.Size_28x90 && !i.IsSheilaFabricBought && i.Status == ItemStatus.ReadyForFabricProcurement)
                .Select(i => new SheilaProcurementItem
                {
                    OrderItemId = i.OrderItemId,
                    OrderId = i.OrderId,
                    SheilaSizeText = "28 x 90 inches (XL Custom)",
                    ModelDescription = i.ModelTextDescription
                })
                .ToListAsync();
        }

        public async Task<bool> MarkFabricAsBoughtAsync(int orderItemId, bool isSheila)
        {
            var item = await _context.OrderItems.FindAsync(orderItemId);
            if (item == null) return false;

            if (isSheila)
            {
                item.IsSheilaFabricBought = true;
            }
            else
            {
                item.IsAbayaFabricBought = true;
            }

            // 🔄 Lifecycle Automation Hook: If both fabric constraints are satisfied, push it straight to the production cutting master queue!
            bool needsAbayaFabric = item.IsShopProvidingFabric;
            bool needsSheilaFabric = item.SelectedSheilaSize == SheilaSize.Size_28x90;

            bool abayaReady = !needsAbayaFabric || item.IsAbayaFabricBought;
            bool sheilaReady = !needsSheilaFabric || item.IsSheilaFabricBought;

            if (abayaReady && sheilaReady)
            {
                item.Status = ItemStatus.ReadyForDispatch; // Moves out of procurement into factory routing assignment pool
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
    }

    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static ServiceResult Success() => new() { IsSuccess = true };
        public static ServiceResult Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
    }

    // Helper model to transfer front-end form data safely
    public class OrderFormModel
    {
        public string ManualOrderId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public OrderType TypeOfOrder { get; set; } = OrderType.CustomOrder;
        public decimal TotalAmount { get; set; }
        public decimal DepositPaid { get; set; }
        public string FabricName { get; set; } = string.Empty;
        public bool IsShopProvidingFabric { get; set; } = true;
        public SheilaSize SelectedSheilaSize { get; set; } = SheilaSize.Size_28x81;
        public string ModelTextDescription { get; set; } = string.Empty;
        public bool IsReadyMadeAlteration { get; set; } = false;
        public string AlterationNotes { get; set; } = string.Empty;
    }

public class FabricProcurementItem
{
    public int OrderItemId { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string FabricName { get; set; } = string.Empty;
    public string ModelDescription { get; set; } = string.Empty;
}

public class SheilaProcurementItem
{
    public int OrderItemId { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string SheilaSizeText { get; set; } = string.Empty;
    public string ModelDescription { get; set; } = string.Empty;
}

