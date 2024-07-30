namespace IMS.UseCases.Products.Interfaces;

public interface IDeleteProductUseCase
{
    Task ExecuteAsync(int productId);
}