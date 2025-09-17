namespace ThreeLayers.Business.Models;

public class Product : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Value { get; set; }
    public DateTime CreationDate { get; set; }
    public bool Active { get; set; }

    /* EF Relational */
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
}
