namespace AbayaSystem.Core;

public class SheilaShopBalance
{
    public SheilaShop Shop { get; set; } = new();
    public decimal TotalPurchased { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal BalanceDue => TotalPurchased - TotalPaid;
}
