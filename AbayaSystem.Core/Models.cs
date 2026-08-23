using System;
using System.Collections.Generic;

namespace AbayaSystem.Core
{
    namespace AbayaSystem.Core
    {
        public class StatusLog
        {
            public int StatusLogId { get; set; }
            public string OrderId { get; set; } = string.Empty;
            public int OrderItemId { get; set; }

            public ItemStatus? PreviousState { get; set; }
            public ItemStatus CurrentState { get; set; }

            public int? PreviousWorkerId { get; set; }
            public int? CurrentWorkerId { get; set; }

            public DateTime TimeOfEvent { get; set; } = DateTime.UtcNow;
            public string? Notes { get; set; }
        }
    }

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

    // 🤝 External Suppliers / Vendors
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    // 🔘 Abaya Button Configuration Options
    public enum ButtonType
    {
        NoButtons,
        ButtonsWithBand,
        ButtonsWithoutBand
    }

    // 👤 Separate Customer Entity with Complete Measurement Profile
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        // 📏 Measurements
        public decimal LengthAbayaFront { get; set; }
        public decimal LengthAbayaBack { get; set; }
        public decimal LengthSleeve { get; set; }
        public decimal WidthArmHole { get; set; }
        public decimal WidthSleeveOpening { get; set; }
        public decimal WidthShoulder { get; set; }
        public decimal WidthBody { get; set; }
        public decimal WidthBottom { get; set; }

        public ButtonType ButtonType { get; set; } = ButtonType.NoButtons;
        public int NumberOfButtons { get; set; }
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

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
    }

    public enum OrderType
    {
        Internal,
        Hybrid,
        External
    }

    public enum SheilaSize
    {
        Size_22x81,
        Size_28x81,
        Size_28x90
    }

    public enum ItemStatus
    {
        // 🧵 Fabric Procurement & Initial Processing
        ReadyForFabricProcurement,   
        // Initial state for Internal/Hybrid jobs, or External when BuyFabricForExternal == true
        QueueRawFabricEmb,           
        // Fabric procured; queued for raw fabric embroidery (Hybrid with rawFabricEmb)
        OutForRawFabricEmb,          
        // Dispatched for raw fabric embroidery

        // ✂️ Cutting Phase
        QueueCut,                    
        // Fabric procured/returned from raw embroidery; queued for cutting

        // 🪡 Half-Stitching Phase
        QueueHalfStitching,          
        // Cut completed; queued for half-stitching (if HandEmbRequired or Hybrid)
        HalfStitchActive,            
        // Currently undergoing half-stitching

        // 🎨 External Embroidery & Hand Embroidery (Hybrid Workflow)
        QueueHalfStitchEmb,          
        // Half-stitched; queued for dispatch to external embroiderer
        OutForHalfStitchEmb,         
        // Dispatched to external embroiderer
        QueueHandEmb,               
        // Half-stitched / returned from embroidery; queued for hand embroidery
        HandEmbActive,               
        // Currently undergoing hand embroidery

        // 🧵 Full-Stitching Phase
        QueueFullStitching,          
        // Preparatory stitching/embroidery finished; queued for final full stitching
        FullStitchActive,            
        // Currently undergoing full stitching

        // 🏬 Full External Vendor Workflow
        QueueExternalVendor,  
        // Queued for dispatch to full external vendor (Initial if BuyFabricForExternal == false, or after fabric procurement)
        OutWithExternalVendor,       
        // Dispatched to full external vendor

        // 📦 Final Storage & Customer Handover
        ReadyAtWorkShop,             // Full stitching completed by central workshop tailor; ready at workshop
        ReadyAtShop,                 // Received at showroom from workshop, or full stitching completed by showroom tailor
        Delivered,
    }

    // 🧾 Parent Order Container
    public class Order
    {
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public string OrderId { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Today;
        public DateTime EstimatedDeliveryDate { get; set; } = DateTime.UtcNow.AddDays(7);
        public DateTime? ActualDeliveryDate { get; set; }

        public bool IsUrgent { get; set; } = false;
        public string Notes { get; set; } = string.Empty;


        public decimal TotalAmount { get; set; }
        public decimal DepositPaid { get; set; }
        public decimal BalanceDue { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }

    // 👗 Individual Garment Line Item
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int BranchId { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public Order? Order { get; set; }
        public string ModelTextDescription { get; set; } = string.Empty;

        public int? FabricShopId { get; set; }
        public FabricShop? FabricShop { get; set; }

        public int? FabricId { get; set; }
        public Fabric? Fabric { get; set; }

        public string ColorCode { get; set; } = string.Empty;

        public SheilaSize SelectedSheilaSize { get; set; } = SheilaSize.Size_28x81;
        public bool IsReadyMadeAlteration { get; set; } = false;
        public string AlterationNotes { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public int TargetBranchId { get; set; }
        public int? AssignedSupplierId { get; set; }
        public Supplier? AssignedSupplier { get; set; }

        public int? CutByWorkerId { get; set; }
        public int? StitchedByWorkerId { get; set; }
        public int? HandEmbroideredByWorkerId { get; set; }

        public OrderType TypeOfOrder { get; set; } = OrderType.Internal;


        public ItemStatus Status { get; set; } = ItemStatus.ReadyForFabricProcurement;

        public bool IsAbayaFabricBought { get; set; } = false;
        public bool IsSheilaFabricBought { get; set; } = false;

        public ItemCategory Category { get; set; } = ItemCategory.Abaya;
        public int? ExternalWorkerId { get; set; }
        public ExternalWorker? ExternalWorker { get; set; }
        public bool BuyFabricForExternal { get; set; } = false;

        public bool HandEmbRequired { get; set; } = false;
        public bool rawFabricEmb { get; set; } = false;
    }

    public class FabricProcurementItem
    {
        public int OrderItemId { get; set; }
        public int BranchId { get; set; }
        public string OrderId { get; set; } = string.Empty;
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
        public string ModelTextDescription { get; set; } = string.Empty;
        public SheilaSize SelectedSheilaSize { get; set; }
        public string FabricShopName { get; set; } = string.Empty;
        public string FabricName { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }
}