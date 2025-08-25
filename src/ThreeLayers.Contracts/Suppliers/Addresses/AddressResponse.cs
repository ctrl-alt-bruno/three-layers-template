using System.Text.Json.Serialization;

namespace ThreeLayers.Contracts.Suppliers.Addresses;

public class AddressResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("street")]
    public required string Street { get; set; }

    [JsonPropertyName("number")]
    public required string Number { get; set; }

    [JsonPropertyName("complement")]
    public string? Complement { get; set; }

    [JsonPropertyName("postal-code")]
    public required string PostalCode { get; set; }

    [JsonPropertyName("region")]
    public required string Region { get; set; }

    [JsonPropertyName("city")]
    public required string City { get; set; }

    [JsonPropertyName("state")]
    public required string State { get; set; }
}
