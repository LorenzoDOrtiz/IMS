using IMS.CoreBusiness;

namespace IMS.UseCases.Interfaces;
public interface IEditInventoryUseCase
{
    Task ExecuteAsync(Inventory inventory);
}