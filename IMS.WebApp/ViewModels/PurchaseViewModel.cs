using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels;

public class PurchaseViewModel
{
    [Required]
    public string PONumber { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "You have to select an inventory.")]
    public int InventoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity has to be greater than or equal to 1.")]
    public int QuantityToPurchase { get; set; }

    public double InventoryPrice { get; set; }
}
