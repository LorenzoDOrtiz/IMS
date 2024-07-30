using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;

namespace IMS.UseCases.Products;
public class ViewProductsByNameUseCase : IViewProductsByNameUseCase
{
    private readonly IProductRepository _productRepository;

    public ViewProductsByNameUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<IEnumerable<Product>> ExecuteAsync(string name = "")
    {
        return await _productRepository.GetProductsByNameAsync(name);
    }
}