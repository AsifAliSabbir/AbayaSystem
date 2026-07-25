using System;
using System.Collections.Generic;

namespace AbayaSystem.Core
{
    // 🏢 Branch / Workshop Locations
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public bool IsWorkshop { get; set; } = false; // True = Central Workshop, False = Showroom
    }

    // 🏬 Fabric Shops Catalog
    public class FabricShop
    {
        public int FabricShopId { get; set; }
        public string FabricShopName { get; set; } = string.Empty;
    }

    // 🧵 Fabric Names Catalog
    public class Fabric
    {
        public int FabricId { get; set; }
        public string FabricName { get; set; } = string.Empty;
    }

    // 🤝 External Suppliers / Vendors (Embroiderers, Full-Abaya Makers, etc.)
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    [Flags]
    public enum WorkerRole
    {
        Salesman = 1,
        CuttingMaster = 2,
        Tailor = 4,
        Admin = 8,
        HandEmbroiderer = 16
    }

    public class Worker
    {
        public int WorkerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public WorkerRole AssignedRoles { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // 🔗 Linked to specific Branch/Showroom or Workshop
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
    }

    // 🏷️ Order Types
    public enum OrderType
    {
        Internal, // Internal Workshop or Showroom tailor only
        Hybrid,   // Internal Workshop + External Embroiderer/Vendor
        External  // 100% External Supplier made
    }

    public enum HybridProcessType
    {
        None,
        CutAndHalfStitchFirst, // Cut -> Half Stitch in Workshop -> External Embroiderer -> In-House Finish
        RawFabricFirst         // Raw Fabric -> External Embroiderer -> In-House Cut & Stitch
    }

    public enum SheilaSize
    {
        Size_22x81, // Standard Stock
        Size_28x81, // Standard Stock
        Size_28x90  // Custom XL - Triggers dedicated fabric purchase
    }

    public enum ItemStatus
    {
        ReadyForFabricProcurement,
        OutForRawFabricEmbroidery,
        AssignedToCutter,
        InStitchingQueue,
        HalfStitchedInWorkshop,
        OutForHalfStitchEmbroidery,
        HandEmbroideryActive,
        QualityCheck,
        OutWithExternalVendor,
        ReadyAtShop,
        AlterationActive,
        Completed,
    }

    // 🧾 Parent Order Container (Uses Composite Key: BranchId + OrderId)
    public class Order
    {
        // 🔑 Composite Primary Key Part 1: Branch where order was created
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        // 🔑 Composite Primary Key Part 2: Manual receipt number (e.g., 45098, RM-101)
        public string OrderId { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Today;
        // 📅 Delivery Tracking
        public DateTime EstimatedDeliveryDate { get; set; } = DateTime.UtcNow.AddDays(7);
        public DateTime? ActualDeliveryDate { get; set; }

        // 🚨 Workflow Priority Flag
        public bool IsUrgent { get; set; } = false;

        // 📝 Order Level Note
        public string Notes { get; set; } = string.Empty;

        public OrderType TypeOfOrder { get; set; } = OrderType.Internal;

        // Financials
        public decimal TotalAmount { get; set; }
        public decimal DepositPaid { get; set; }
        public decimal BalanceDue => TotalAmount - DepositPaid;

        public List<OrderItem> Items { get; set; } = new();
    }

    // 👗 Individual Garment Line Item
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        // 🔗 Foreign Key pointing back to Composite Parent Order
        public int BranchId { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public Order? Order { get; set; }
        public string ModelTextDescription { get; set; } = string.Empty;

        // 🏬 Dropdown Selections
        public int? FabricShopId { get; set; }
        public FabricShop? FabricShop { get; set; }

        public int? FabricId { get; set; }
        public Fabric? Fabric { get; set; }

        // Whole number code from catalogue (e.g., "1", "2", "15")
        public string ColorCode { get; set; } = string.Empty;

        public bool IsShopProvidingFabric { get; set; } = true;

        // Process details
        public HybridProcessType HybridProcess { get; set; } = HybridProcessType.None;

        // Sizing & Sheila
        public SheilaSize SelectedSheilaSize { get; set; } = SheilaSize.Size_28x81;
        public bool IsReadyMadeAlteration { get; set; } = false;
        public string AlterationNotes { get; set; } = string.Empty;

        // 📝 Item Level Note
        public string Notes { get; set; } = string.Empty;

        // Logistics & Routing
        public int TargetBranchId { get; set; } // Workshop Branch ID or Local Showroom Branch ID
        public int? AssignedSupplierId { get; set; } // External Computer Embroiderer / Vendor
        public Supplier? AssignedSupplier { get; set; }

        public int? CutByWorkerId { get; set; }
        public int? StitchedByWorkerId { get; set; }
        public int? HandEmbroideredByWorkerId { get; set; }

        public ItemStatus Status { get; set; } = ItemStatus.ReadyForFabricProcurement;

        public bool IsAbayaFabricBought { get; set; } = false;
        public bool IsSheilaFabricBought { get; set; } = false;

        public ItemCategory Category { get; set; } = ItemCategory.Abaya;
        public int? ExternalWorkerId { get; set; }
        public ExternalWorker? ExternalWorker { get; set; }
        public bool BuyFabricForExternal { get; set; } = false;
    }

    // --- Add these two procurement models for FabricProcurement.razor ---
    public class FabricProcurementItem
    {
        public int OrderItemId { get; set; }
        public int BranchId { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ModelDescription { get; set; } = string.Empty;
        public string FabricShopName { get; set; } = string.Empty;
        public string FabricName { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }

    public class SheilaProcurementItem
    {
        public int OrderItemId { get; set; }
        public int BranchId { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ModelTextDescription { get; set; } = string.Empty;
        public SheilaSize SelectedSheilaSize { get; set; }
        public DateTime OrderDate { get; set; }
    }
}