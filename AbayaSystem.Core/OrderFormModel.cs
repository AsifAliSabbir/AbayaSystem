using System;
using System.Collections.Generic;

namespace AbayaSystem.Core
{
    public class OrderFormModel
    {
        // 🏢 Master Order Properties
        public int BranchId { get; set; }
        public string ManualOrderId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Today; // 📅 1. Default to Current Date
        public DateTime EstimatedDeliveryDate { get; set; } = DateTime.Today.AddDays(7);
        public bool IsUrgent { get; set; } = false;
        public string OrderNotes { get; set; } = string.Empty;

        // 💰 Financials
        public decimal TotalAmount { get; set; }
        public decimal DepositPaid { get; set; }

        // 👗 3. List of Items in Order
        public List<OrderItemFormModel> Items { get; set; } = new();
    }

    public class OrderItemFormModel
    {
        // 👗 4. Item Category (Default: Abaya)
        public ItemCategory Category { get; set; } = ItemCategory.Abaya;
        public string ModelTextDescription { get; set; } = string.Empty;

        // 🏬 Dropdowns & Color Code
        public int? FabricShopId { get; set; }
        public int? FabricId { get; set; }
        public string ColorCode { get; set; } = "Black"; // 🎨 7. Default to Black

        // 🧵 6. Selected Workflow String Key (e.g., "Internal", "Hybrid_1", "External_2")
        public string SelectedWorkflowKey { get; set; } = "Internal";
        public HybridProcessType HybridProcess { get; set; } = HybridProcessType.None;
        public bool BuyFabricForExternal { get; set; } = false; // 🧵 Checkbox for External Fabric Purchase

        public SheilaSize SelectedSheilaSize { get; set; } = SheilaSize.Size_28x81;

        // ✂️ Alterations & Routing
        public bool IsReadyMadeAlteration { get; set; } = false;
        public string AlterationNotes { get; set; } = string.Empty;
        public string ItemNotes { get; set; } = string.Empty;
        public int TargetBranchId { get; set; }
    }
}