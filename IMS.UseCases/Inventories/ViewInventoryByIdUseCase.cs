using IMS.CoreBusiness;
using IMS.UseCases.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories;
public class ViewInventoryByIdUseCase : IViewInventoryByIdUseCase
{
    private readonly IInventoryRepository _inventoryRepository;

    public ViewInventoryByIdUseCase(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<Inventory> ExecuteAsync(int inventoryId)
    {
        return await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
    }
}
