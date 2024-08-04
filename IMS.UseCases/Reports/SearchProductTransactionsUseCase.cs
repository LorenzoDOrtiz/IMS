using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.Interfaces;

namespace IMS.UseCases.Reports;
public class SearchProductTransactionsUseCase : ISearchProductTransactionsUseCase
{
    private readonly IProductTransactionRepository _productTransactionRepository;

    public SearchProductTransactionsUseCase(IProductTransactionRepository productTransactionRepository)
    {
        _productTransactionRepository = productTransactionRepository;
    }

    public async Task<IEnumerable<ProductTransaction>> ExecuteAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
    {
        if (dateTo.HasValue) dateTo = dateTo.Value.AddDays(1);

        return await _productTransactionRepository.GetProductTransactionsAsync(productName, dateFrom, dateTo, transactionType);
    }
}
