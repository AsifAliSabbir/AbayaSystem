namespace AbayaSystem.Core
{
    public class OrderFormModel
    {
        // 🏢 Parent Order Properties
        public int BranchId { get; set; }
        public string ManualOrderId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public OrderType TypeOfOrder { get; set; } = OrderType.Internal;

        // 📅 Dates & Urgency
        public DateTime EstimatedDeliveryDate { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsUrgent { get; set; } = false;
        public string OrderNotes { get; set; } = string.Empty;

        // 💰 Financials
        public decimal TotalAmount { get; set; }
        public decimal DepositPaid { get; set; }

        // 👗 Garment Line Item Properties
        public string ModelTextDescription { get; set; } = string.Empty;

        // 🏬 Dropdown Selections
        public int? FabricShopId { get; set; }
        public int? FabricId { get; set; }
        public string ColorCode { get; set; } = string.Empty;

        // 🧵 Process & Sizing
        public HybridProcessType HybridProcess { get; set; } = HybridProcessType.None;
        public SheilaSize SelectedSheilaSize { get; set; } = SheilaSize.Size_28x81;

        // ✂️ Alterations & Routing
        public bool IsReadyMadeAlteration { get; set; } = false;
        public string AlterationNotes { get; set; } = string.Empty;
        public string ItemNotes { get; set; } = string.Empty;
        public int TargetBranchId { get; set; }
    }

    // 📦 Helper DTOs for Procurement Lists
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
}