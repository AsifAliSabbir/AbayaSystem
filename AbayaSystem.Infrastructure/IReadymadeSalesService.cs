using AbayaSystem.Core;
using Microsoft.EntityFrameworkCore;

namespace AbayaSystem.Infrastructure;

public interface IReadymadeSalesService
{
    Task<List<Branch>> GetBranchesAsync();
    Task<List<ReadymadeSale>> GetSalesAsync(int? branchId = null, DateTime? dateFrom = null, DateTime? dateTo = null);
    Task<ReadymadeSale> CreateSaleAsync(ReadymadeSale sale);
}

public class ReadymadeSalesService : IReadymadeSalesService
{
    private readonly BoutiqueDbContext _context;

    public ReadymadeSalesService(BoutiqueDbContext context) => _context = context;

    public Task<List<Branch>> GetBranchesAsync() =>
        _context.Branches.AsNoTracking().Where(b => !b.IsWorkshop).OrderBy(b => b.BranchName).ToListAsync();

    public Task<List<ReadymadeSale>> GetSalesAsync(int? branchId = null, DateTime? dateFrom = null, DateTime? dateTo = null) =>
        _context.ReadymadeSales.AsNoTracking()
            .Include(s => s.Branch)
            .Where(s => !branchId.HasValue || s.BranchId == branchId.Value)
            .Where(s => !dateFrom.HasValue || s.SaleDateTime >= dateFrom.Value)
            .Where(s => !dateTo.HasValue || s.SaleDateTime < dateTo.Value.AddDays(1))
            .OrderByDescending(s => s.SaleDateTime)
            .ThenByDescending(s => s.ReadymadeSaleId)
            .ToListAsync();

    public async Task<ReadymadeSale> CreateSaleAsync(ReadymadeSale sale)
    {
        if (sale == null) throw new ArgumentNullException(nameof(sale));
        if (sale.BranchId <= 0) throw new ArgumentException("Branch is required.");
        if (string.IsNullOrWhiteSpace(sale.Description)) throw new ArgumentException("Description is required.");
        if (sale.Quantity <= 0 || sale.Quantity != decimal.Truncate(sale.Quantity)) throw new ArgumentException("Quantity must be a positive whole number.");
        if (sale.TotalAmount < 0) throw new ArgumentException("Total amount cannot be negative.");

        sale.Description = sale.Description.Trim();
        sale.SaleDateTime = sale.SaleDateTime == default ? DateTime.UtcNow : sale.SaleDateTime.ToUniversalTime();
        _context.ReadymadeSales.Add(sale);
        await _context.SaveChangesAsync();
        return sale;
    }
}
