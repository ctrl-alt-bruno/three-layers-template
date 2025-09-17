using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ThreeLayers.Contracts.Suppliers.Addresses;

namespace ThreeLayers.Contracts.Suppliers;

public class SupplierUpdateRequest
{
    [Required]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [Required]
    [StringLength(20)]
    [JsonPropertyName("document")]
    public required string Document { get; set; }

    [Required]
    [JsonPropertyName("supplier_type")]
    public int SupplierType { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [Required]
    [JsonPropertyName("address")]
    public AddressUpdateRequest? Address { get; set; }
}
