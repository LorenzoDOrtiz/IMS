using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Plugin;
public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
{
    private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public PurchaseInventoryUseCase(IInventoryTransactionRepository inventoryTransactionRepository,
        IInventoryRepository inventoryRepository)
    {
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _inventoryRepository = inventoryRepository;
    }
    public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
    {
        // Insert a record in the transaction table 
        _inventoryTransactionRepository.PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);

        // Increate the quantity
        inventory.Quantity += quantity;
        await _inventoryRepository.UpdateInventoryAsync(inventory);
    }
}
