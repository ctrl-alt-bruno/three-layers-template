namespace ThreeLayers.Business.Models;

public class Supplier : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public SupplierTypes SupplierType { get; set; }
    public bool Active { get; set; }
    public Address? Address { get; set; }
    public IEnumerable<Product> Products { get; set; } = [];
}