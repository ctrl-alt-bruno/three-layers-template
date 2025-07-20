namespace ThreeLayers.Contracts.Suppliers.Addresses;

public class AddressResponse
{
    public Guid Id { get; set; }

    public required string Street { get; set; }

    public required string Number { get; set; }

    public string? Complement { get; set; }

    public required string PostalCode { get; set; }

    public required string Region { get; set; }

    public required string City { get; set; }

    public required string State { get; set; }
}