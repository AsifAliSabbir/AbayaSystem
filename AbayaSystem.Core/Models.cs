using System;
using System.Collections.Generic;
using System.Text;

namespace AbayaSystem.Core
{
    [Flags]
    public enum WorkerRole
    {
        Salesman = 1,
        CuttingMaster = 2,
        Tailor = 4,
        Admin = 8,
        HandEmbroiderer
    }

    public class Worker
    {
        public int WorkerId { get; set; } // Auto-incrementing internal ID
        public string Name { get; set; } = string.Empty;

        // This holds the added math values of their combined roles (e.g., 6 = Cutter + Tailor)
        public WorkerRole AssignedRoles { get; set; }

        // 🔒 New Authentication Fields
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Never store plain text passwords!
    }


    // 🏷️ Transaction and Tracking Pipelines
    public enum OrderType
    {
        CustomOrder,                // Standard handwritten receipt customer build
        ReadyMadeRetail,            // Customer bought the physical display sample
        InternalStockReplenishment  // Manual factory re-make ticket (AED 0 value)
    }

    public enum SheilaSize
    {
        Size_22x81, // Standard Stock
        Size_28x81, // Standard Stock
        Size_28x90  // Custom XL - 🚨 Triggers dedicated fabric purchase
    }

    public enum ItemStatus
    {
        ReadyForFabricProcurement,
        ReadyForDispatch,
        AssignedToCutter,
        InStitchingQueue,
        StitchingActive,
        HandEmbroideryActive,
        QualityCheck,
        OutWithExternalWorkshop,
        ReadyAtShop,
        AlterationActive,
        Closed
    }

    // 🧾 Parent Order Container
    public class Order
    {
        // Pure manual entry text identifier (e.g., 45098, RM-45098, WM-502)
        public string OrderId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime DateOrdered { get; set; } = DateTime.UtcNow;
        public OrderType TypeOfOrder { get; set; } = OrderType.CustomOrder;

        // Financials
        public decimal TotalAmount { get; set; }
        public decimal DepositPaid { get; set; }
        public decimal BalanceDue => TotalAmount - DepositPaid;

        // One order sheet can hold multiple customized abaya pieces
        public List<OrderItem> Items { get; set; } = new();
    }

    // 👗 Individual Garment Line Item
    public class OrderItem
    {
        public int OrderItemId { get; set; } // Auto-incrementing line ID
        public string OrderId { get; set; } = string.Empty; // Maps back to your manual ID

        // Free-text descriptors matching receipt workflow
        public string ModelTextDescription { get; set; } = string.Empty;
        public string FabricName { get; set; } = string.Empty;
        public bool IsShopProvidingFabric { get; set; } = true;

        // Sizing and Alterations
        public SheilaSize SelectedSheilaSize { get; set; } = SheilaSize.Size_28x81;
        public bool IsReadyMadeAlteration { get; set; } = false;
        public string AlterationNotes { get; set; } = string.Empty;

        // Logistics and Factory Routing Assignments
        public int AssignedWorkshopId { get; set; } = 1;
        public int? CutByWorkerId { get; set; }
        public int? StitchedByWorkerId { get; set; }
        public ItemStatus Status { get; set; } = ItemStatus.ReadyForFabricProcurement;

        /// <summary>
        /// Tracks if the shop-provided fabric has been purchased from the market.
        /// </summary>
        public bool IsAbayaFabricBought { get; set; } = false;

        /// <summary>
        /// Tracks if the custom 28x90 XL Sheila fabric has been secured.
        /// </summary>
        public bool IsSheilaFabricBought { get; set; } = false;
    }
}
