using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ThreeLayers.Contracts.Suppliers.Addresses;

public class AddressUpdateRequest
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [Required]
    [StringLength(200)]
    [JsonPropertyName("street")]
    public required string Street { get; set; }

    [Required]
    [StringLength(50)]
    [JsonPropertyName("number")]
    public required string Number { get; set; }

    [JsonPropertyName("complement")]
    public string? Complement { get; set; }

    [Required]
    [StringLength(10)]
    [JsonPropertyName("postal_code")]
    public required string PostalCode { get; set; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("region")]
    public required string Region { get; set; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("city")]
    public required string City { get; set; }

    [Required]
    [StringLength(10)]
    [JsonPropertyName("state")]
    public required string State { get; set; }
}
