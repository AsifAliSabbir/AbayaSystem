namespace AbayaSystem.Core
{
    public class CustomerSearchResultDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string LastOrderId { get; set; } = string.Empty;

        // Measurements
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
}