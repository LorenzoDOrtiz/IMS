using IMS.UseCases.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories;
public class DeleteInventoryUseCase : IDeleteInventoryUseCase
{
    private readonly IInventoryRepository _inventoryRepository;

    public DeleteInventoryUseCase(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task ExecuteAsync(int inventoryId)
    {
        await _inventoryRepository.DeleteInventoryByIdAsync(inventoryId);
    }
}
