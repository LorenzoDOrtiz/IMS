using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.Interfaces;

namespace IMS.UseCases.Reports;
public class SearchInventoryTransactionsUseCase : ISearchInventoryTransactionsUseCase
{
    private readonly IInventoryTransactionRepository _inventoryTransactionRepository;

    public SearchInventoryTransactionsUseCase(IInventoryTransactionRepository inventoryTransactionRepository)
    {
        _inventoryTransactionRepository = inventoryTransactionRepository;
    }

    public async Task<IEnumerable<InventoryTransaction>> ExecuteAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
    {
        if (dateTo.HasValue) dateTo = dateTo.Value.AddDays(1);

        return await _inventoryTransactionRepository.GetInventoryTransactionsAsync(inventoryName, dateFrom, dateTo, transactionType);
    }
}
