using System;
using System.Collections.Generic;

namespace AbayaSystem.Core
{
    public class OrderFormModel
    {
        // 🏢 Master Order Properties
        public int BranchId { get; set; }
        public string ManualOrderId { get; set; } = string.Empty;

        // 👤 Linked Customer Details & Search Binding
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        // 📏 Customer Measurements Profile
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

        public DateTime OrderDate { get; set; } = DateTime.Today;
        public DateTime EstimatedDeliveryDate { get; set; } = DateTime.Today.AddDays(7);
        public bool IsUrgent { get; set; } = false;
        public string OrderNotes { get; set; } = string.Empty;

        // 💰 Financials
        public decimal TotalAmount { get; set; }
        public decimal DepositPaid { get; set; }
        public decimal BalanceDue => TotalAmount - DepositPaid;

        // 👗 List of Items in Order
        public List<OrderItemFormModel> Items { get; set; } = new();
    }

    public class OrderItemFormModel
    {
        public ItemCategory Category { get; set; } = ItemCategory.Abaya;
        public string ModelTextDescription { get; set; } = string.Empty;

        public int? FabricShopId { get; set; }
        public int? FabricId { get; set; }
        public string ColorCode { get; set; } = "Black";

        public string SelectedWorkflowKey { get; set; } = "Internal";
        public HybridProcessType HybridProcess { get; set; } = HybridProcessType.None;
        public bool BuyFabricForExternal { get; set; } = false;

        public SheilaSize SelectedSheilaSize { get; set; } = SheilaSize.Size_28x81;

        public bool IsReadyMadeAlteration { get; set; } = false;
        public string AlterationNotes { get; set; } = string.Empty;
        public string ItemNotes { get; set; } = string.Empty;
        public int TargetBranchId { get; set; }
    }
}