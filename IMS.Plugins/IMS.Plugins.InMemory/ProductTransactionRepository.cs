using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory;
public class ProductTransactionRepository : IProductTransactionRepository
{
    private List<ProductTransaction> _productTransactions = new();
    private readonly IProductRepository _productRepository;
    private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public ProductTransactionRepository(IProductRepository productRepository,
        IInventoryTransactionRepository inventoryTransactionRepository,
        IInventoryRepository inventoryRepository)
    {
        _productRepository = productRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _inventoryRepository = inventoryRepository;
    }

    public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
    {
        // decrease the inventories
        var prod = await _productRepository.GetProductByIdAsync(product.ProductId);

        if (prod != null)
        {
            foreach (var pi in prod.ProductInventories)
            {
                if (pi.Inventory is not null)
                {
                    _inventoryTransactionRepository.ProduceAsync(productionNumber,
                    pi.Inventory,
                    pi.InventoryQuantity * quantity,
                    doneBy,
                    -1);

                    var inv = await _inventoryRepository.GetInventoryByIdAsync(pi.InventoryId);
                    inv.Quantity -= pi.InventoryQuantity * quantity;

                    await _inventoryRepository.UpdateInventoryAsync(inv);
                }
            }
        }

        // add product transaction

        _productTransactions.Add(new ProductTransaction
        {
            ProductionNumber = productionNumber,
            ProductId = product.ProductId,
            QuantityBefore = product.Quantity,
            ActivityType = ProductTransactionType.ProduceProduct,
            QuantityAfter = product.Quantity + quantity,
            TransactionDate = DateTime.UtcNow,
            DoneBy = doneBy
        });
    }

    public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
    {
        _productTransactions.Add(new ProductTransaction
        {
            ActivityType = ProductTransactionType.SellProduct,
            SONumber = salesOrderNumber,
            ProductId = product.ProductId,
            QuantityBefore = product.Quantity,
            QuantityAfter = product.Quantity - quantity,
            TransactionDate = DateTime.UtcNow,
            DoneBy = doneBy,
            UnitPrice = unitPrice
        });

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
    {
        var products = (await _productRepository.GetProductsByNameAsync(string.Empty)).ToList();

        var query = from it in _productTransactions
                    join inv in products on it.ProductId equals inv.ProductId
                    where
                        (string.IsNullOrEmpty(productName) || inv.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                        &&
                        (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                        (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                        (!transactionType.HasValue || it.ActivityType == transactionType)
                    select new ProductTransaction
                    {
                        Product = inv,
                        ProductTransactionId = it.ProductTransactionId,
                        SONumber = it.SONumber,
                        ProductionNumber = it.ProductionNumber,
                        ProductId = it.ProductId,
                        QuantityBefore = it.QuantityBefore,
                        ActivityType = it.ActivityType,
                        QuantityAfter = it.QuantityAfter,
                        TransactionDate = it.TransactionDate,
                        DoneBy = it.DoneBy,
                        UnitPrice = it.UnitPrice
                    };

        return query;
    }
}
