using AbayaSystem.Core;
using Microsoft.EntityFrameworkCore;

namespace AbayaSystem.Infrastructure;

public interface ISheilaService
{
    Task<List<SheilaShop>> GetShopsAsync(int? branchId = null);
    Task<SheilaShop> CreateShopAsync(SheilaShop shop);
    Task UpdateShopAsync(SheilaShop shop);
    Task<List<SheilaTran>> GetTransactionsAsync(int? branchId = null);
    Task<List<SheilaShopBalance>> GetBalancesAsync(int? branchId = null);
    Task<SheilaTran> CreateTransactionAsync(SheilaTran transaction);
}

public class SheilaService : ISheilaService
{
    private const decimal SheilaUnitPrice = 20m;
    private readonly BoutiqueDbContext _context;

    public SheilaService(BoutiqueDbContext context)
    {
        _context = context;
    }

    public async Task<List<SheilaShop>> GetShopsAsync(int? branchId = null) =>
        await _context.SheilaShops
            .AsNoTracking()
            .Include(s => s.Branch)
            .Where(s => !branchId.HasValue || s.BranchID == branchId.Value)
            .OrderBy(s => s.SheilaShopName)
            .ToListAsync();

    public async Task<SheilaShop> CreateShopAsync(SheilaShop shop)
    {
        if (shop == null) throw new ArgumentNullException(nameof(shop));
        if (string.IsNullOrWhiteSpace(shop.SheilaShopName)) throw new ArgumentException("Shop name is required.");
        if (shop.BranchID <= 0) throw new ArgumentException("Branch is required.");

        shop.SheilaShopName = shop.SheilaShopName.Trim();
        _context.SheilaShops.Add(shop);
        await _context.SaveChangesAsync();
        return shop;
    }

    public async Task UpdateShopAsync(SheilaShop shop)
    {
        var existing = await _context.SheilaShops.FindAsync(shop.SheilaShopID)
            ?? throw new KeyNotFoundException("Sheila shop not found.");

        existing.SheilaShopName = shop.SheilaShopName.Trim();
        existing.BranchID = shop.BranchID;
        await _context.SaveChangesAsync();
    }

    public async Task<List<SheilaTran>> GetTransactionsAsync(int? branchId = null) =>
        await _context.SheilaTrans
            .AsNoTracking()
            .Include(t => t.SheilaShop)
                .ThenInclude(s => s!.Branch)
            .Where(t => !branchId.HasValue || t.SheilaShop!.BranchID == branchId.Value)
            .OrderByDescending(t => t.TransDateTime)
            .ThenByDescending(t => t.SheilaTranID)
            .ToListAsync();

    public async Task<List<SheilaShopBalance>> GetBalancesAsync(int? branchId = null)
    {
        var shops = await GetShopsAsync(branchId);
        var totals = await _context.SheilaTrans
            .AsNoTracking()
            .Where(t => !branchId.HasValue || t.SheilaShop!.BranchID == branchId.Value)
            .GroupBy(t => t.SheilaShopID)
            .Select(g => new
            {
                SheilaShopID = g.Key,
                TotalPurchased = g.Sum(t => t.PurchaseAmount),
                TotalPaid = g.Sum(t => t.PaymentAmount)
            })
            .ToDictionaryAsync(x => x.SheilaShopID);

        return shops.Select(shop =>
        {
            totals.TryGetValue(shop.SheilaShopID, out var total);
            return new SheilaShopBalance
            {
                Shop = shop,
                TotalPurchased = total?.TotalPurchased ?? 0,
                TotalPaid = total?.TotalPaid ?? 0
            };
        }).ToList();
    }

    public async Task<SheilaTran> CreateTransactionAsync(SheilaTran transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction.SheilaShopID <= 0) throw new ArgumentException("Sheila shop is required.");
        if (transaction.Quantity < 0 || transaction.Quantity != decimal.Truncate(transaction.Quantity) || transaction.PaymentAmount < 0)
            throw new ArgumentException("Quantity and amounts cannot be negative.");
        transaction.PurchaseAmount = transaction.Quantity * SheilaUnitPrice;
        if (transaction.PurchaseAmount == 0 && transaction.PaymentAmount == 0)
            throw new ArgumentException("Enter a purchase amount or payment amount.");
        if (transaction.PaymentAmount > transaction.PurchaseAmount && transaction.PurchaseAmount > 0)
            throw new ArgumentException("Payment cannot exceed the purchase amount on one transaction.");

        transaction.OrderID = string.IsNullOrWhiteSpace(transaction.OrderID) ? null : transaction.OrderID.Trim();
        transaction.TransDateTime = transaction.TransDateTime == default ? DateTime.UtcNow : transaction.TransDateTime;
        _context.SheilaTrans.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }
}
