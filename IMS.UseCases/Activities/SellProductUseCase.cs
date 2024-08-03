using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities;
public class SellProductUseCase : ISellProductUseCase
{
    private readonly IProductTransactionRepository _productTransactionRepository;
    private readonly IProductRepository _productRepository;

    public SellProductUseCase(IProductTransactionRepository productTransactionRepository,
        IProductRepository productRepository)
    {
        _productTransactionRepository = productTransactionRepository;
        _productRepository = productRepository;
    }

    public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
    {
        await _productTransactionRepository.SellProductAsync(salesOrderNumber, product, quantity, unitPrice, doneBy);

        product.Quantity -= quantity;

        _productRepository.UpdateProductAsync(product);
    }
}
