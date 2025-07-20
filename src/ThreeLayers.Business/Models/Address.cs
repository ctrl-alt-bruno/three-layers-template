namespace ThreeLayers.Business.Models;

public class Address : Entity
{
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string? Complement { get; set; } = null;
    public string PostalCode { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;

    /* EF Relational */
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
}