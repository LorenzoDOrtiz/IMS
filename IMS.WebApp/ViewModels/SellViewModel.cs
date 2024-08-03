using IMS.CoreBusiness;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels;

public class SellViewModel
{
    [Required]
    public string SalesOrderNumber { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "You have to select a product.")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity has to be greater or equal to 1.")]
    public int QuantityToSell { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Price has to be greater than or equal to 0.")]
    public double UnitPrice { get; set; }
    public Product? Product { get; set; }
}