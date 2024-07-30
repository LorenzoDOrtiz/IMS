using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;

namespace IMS.UseCases.Products;
public class EditProductUseCase : IEditProductUseCase
{
    private readonly IProductRepository _productRepository;

    public EditProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task ExecuteAsync(Product product)
    {
        await _productRepository.UpdateProductAsync(product);
    }
}
