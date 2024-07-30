using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;

namespace IMS.UseCases.Products;
public class DeleteProductUseCase : IDeleteProductUseCase
{
    private readonly IProductRepository _productRepository;

    public DeleteProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task ExecuteAsync(int productId)
    {
        await _productRepository.DeleteProductByIdAsync(productId);
    }
}
