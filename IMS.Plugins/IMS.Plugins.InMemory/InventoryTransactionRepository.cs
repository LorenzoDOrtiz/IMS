using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory;
public class InventoryTransactionRepository : IInventoryTransactionRepository
{
    private readonly IInventoryRepository _inventoryRepository;
    public List<InventoryTransaction> _inventoryTransactions = new();

    public InventoryTransactionRepository(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
    {
        var inventories = (await _inventoryRepository.GetInventoriesByNameAsync(string.Empty)).ToList();

        var query = from it in _inventoryTransactions
                    join inv in inventories on it.InventoryId equals inv.InventoryId
                    where
                        (string.IsNullOrEmpty(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                        &&
                        (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                        (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                        (!transactionType.HasValue || it.ActivityType == transactionType)
                    select new InventoryTransaction
                    {
                        Inventory = inv,
                        InventoryTransactionId = it.InventoryTransactionId,
                        PONumber = it.PONumber,
                        InventoryId = it.InventoryId,
                        QuantityBefore = it.QuantityBefore,
                        ActivityType = it.ActivityType,
                        QuantityAfter = it.QuantityAfter,
                        TransactionDate = it.TransactionDate,
                        DoneBy = it.DoneBy,
                        UnitPrice = it.UnitPrice
                    };

        return query;
    }

    public void ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
    {
        _inventoryTransactions.Add(new InventoryTransaction
        {
            ProductionNumber = productionNumber,
            InventoryId = inventory.InventoryId,
            QuantityBefore = inventory.Quantity,
            ActivityType = InventoryTransactionType.ProduceProduct,
            QuantityAfter = inventory.Quantity - quantityToConsume,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = price
        });
    }

    public void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
    {
        _inventoryTransactions.Add(new InventoryTransaction
        {
            PONumber = poNumber,
            InventoryId = inventory.InventoryId,
            QuantityBefore = inventory.Quantity,
            ActivityType = InventoryTransactionType.PurchaseInventory,
            QuantityAfter = inventory.Quantity + quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = price
        });
    }
}
