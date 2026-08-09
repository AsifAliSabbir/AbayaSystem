namespace AbayaSystem.Core;

public interface IFabricManagementService
{
    Task<List<FabricShop>> GetFabricShopsAsync();
    Task<FabricShop> CreateFabricShopAsync(FabricShop shop);
    Task UpdateFabricShopAsync(FabricShop shop);
    Task DeleteFabricShopAsync(int fabricShopId);

    Task<List<Fabric>> GetFabricsAsync();
    Task<Fabric> CreateFabricAsync(Fabric fabric);
    Task UpdateFabricAsync(Fabric fabric);
    Task DeleteFabricAsync(int fabricId);
}