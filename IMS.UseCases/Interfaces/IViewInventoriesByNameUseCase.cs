using IMS.CoreBusiness;

namespace IMS.UseCases.Interfaces;
public interface IViewInventoriesByNameUseCase
{
    Task<IEnumerable<Inventory>> ExecuteAsync(string name = "");
}