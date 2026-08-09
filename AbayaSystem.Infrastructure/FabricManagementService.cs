using AbayaSystem.Core;
using Microsoft.EntityFrameworkCore;

namespace AbayaSystem.Infrastructure;

public class FabricManagementService : IFabricManagementService
{
    private readonly BoutiqueDbContext _context;

    public FabricManagementService(BoutiqueDbContext context)
    {
        _context = context;
    }

    // --- FABRIC SUPPLIERS / SHOPS ---
    public async Task<List<FabricShop>> GetFabricShopsAsync()
    {
        return await _context.FabricShops.AsNoTracking().ToListAsync();
    }

    public async Task<FabricShop> CreateFabricShopAsync(FabricShop shop)
    {
        _context.FabricShops.Add(shop);
        await _context.SaveChangesAsync();
        return shop;
    }

    public async Task UpdateFabricShopAsync(FabricShop shop)
    {
        var existing = await _context.FabricShops.FindAsync(shop.FabricShopId);
        if (existing == null)
        {
            throw new KeyNotFoundException("Fabric shop supplier not found.");
        }

        existing.FabricShopName = shop.FabricShopName;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFabricShopAsync(int fabricShopId)
    {
        var shop = await _context.FabricShops.FindAsync(fabricShopId);
        if (shop != null)
        {
            _context.FabricShops.Remove(shop);
            await _context.SaveChangesAsync();
        }
    }

    // --- FABRIC TYPES ---
    public async Task<List<Fabric>> GetFabricsAsync()
    {
        return await _context.Fabrics.AsNoTracking().ToListAsync();
    }

    public async Task<Fabric> CreateFabricAsync(Fabric fabric)
    {
        _context.Fabrics.Add(fabric);
        await _context.SaveChangesAsync();
        return fabric;
    }

    public async Task UpdateFabricAsync(Fabric fabric)
    {
        var existing = await _context.Fabrics.FindAsync(fabric.FabricId);
        if (existing == null)
        {
            throw new KeyNotFoundException("Fabric type not found.");
        }

        existing.FabricName = fabric.FabricName;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFabricAsync(int fabricId)
    {
        var fabric = await _context.Fabrics.FindAsync(fabricId);
        if (fabric != null)
        {
            _context.Fabrics.Remove(fabric);
            await _context.SaveChangesAsync();
        }
    }
}