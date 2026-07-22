using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // 👈 CRITICAL: Fixes CS0411 for ToListAsync and AnyAsync
using AbayaSystem.Core;

namespace AbayaSystem.Infrastructure
{
    public interface IOrderService
    {
        Task<List<FabricShop>> GetFabricShopsAsync();
        Task<List<Fabric>> GetFabricsAsync();
        Task<List<Branch>> GetBranchesAsync();
        Task<ServiceResult> CreateOrderAsync(OrderFormModel model);
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

        public async Task<List<FabricShop>> GetFabricShopsAsync() =>
            await _context.FabricShops.OrderBy(s => s.FabricShopName).ToListAsync();

        public async Task<List<Fabric>> GetFabricsAsync() =>
            await _context.Fabrics.OrderBy(f => f.FabricName).ToListAsync();

        public async Task<List<Branch>> GetBranchesAsync() =>
            await _context.Branches.ToListAsync();

        public async Task<ServiceResult> CreateOrderAsync(OrderFormModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ManualOrderId))
            {
                return ServiceResult.Failure("You must specify a valid manual order number!");
            }

            var cleanId = model.ManualOrderId.Trim().ToUpper();

            // Checks composite key duplicate (BranchId + OrderId)
            var exist = await _context.Orders
                .AnyAsync(o => o.BranchId == model.BranchId && o.OrderId == cleanId);

            if (exist)
            {
                return ServiceResult.Failure($"Order number '{cleanId}' already exists for this branch.");
            }

            var order = new Order
            {
                BranchId = model.BranchId,
                OrderId = cleanId,
                CustomerName = model.CustomerName,
                TypeOfOrder = model.TypeOfOrder,
                EstimatedDeliveryDate = model.EstimatedDeliveryDate,
                IsUrgent = model.IsUrgent,
                Notes = model.OrderNotes,
                TotalAmount = model.TotalAmount,
                DepositPaid = model.DepositPaid
            };

            var orderItem = new OrderItem
            {
                BranchId = model.BranchId,
                OrderId = cleanId,
                ModelTextDescription = model.ModelTextDescription,
                FabricShopId = model.FabricShopId,
                FabricId = model.FabricId,
                ColorCode = model.ColorCode,
                SelectedSheilaSize = model.SelectedSheilaSize,
                IsReadyMadeAlteration = model.IsReadyMadeAlteration,
                AlterationNotes = model.AlterationNotes,
                Notes = model.ItemNotes,
                HybridProcess = model.HybridProcess,
                TargetBranchId = model.TargetBranchId,
                Status = ItemStatus.ReadyForFabricProcurement
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
                    FabricName = i.Fabric != null ? i.Fabric.FabricName : "Custom",
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

            bool needsAbayaFabric = item.IsShopProvidingFabric;
            bool needsSheilaFabric = item.SelectedSheilaSize == SheilaSize.Size_28x90;

            bool abayaReady = !needsAbayaFabric || item.IsAbayaFabricBought;
            bool sheilaReady = !needsSheilaFabric || item.IsSheilaFabricBought;

            if (abayaReady && sheilaReady)
            {
                item.Status = ItemStatus.AssignedToCutter;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }

    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static ServiceResult Success() => new() { IsSuccess = true };
        public static ServiceResult Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
    }
}