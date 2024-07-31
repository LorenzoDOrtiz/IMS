using System.Text.Json.Serialization;

namespace IMS.CoreBusiness;
public class ProductInventory
{
    public int ProductId { get; set; }

    [JsonIgnore]
    public Product? Product { get; set; }

    public int InventoryId { get; set; }

    [JsonIgnore]
    public Inventory? Inventory { get; set; }

    public int InventoryQuantity { get; set; }
}