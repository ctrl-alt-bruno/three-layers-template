using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ThreeLayers.Contracts.Products;

public class ProductCreateRequest
{
    [Required]
    [StringLength(200)]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [StringLength(2000)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [Required]
    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [Required]
    [JsonPropertyName("supplier_id")]
    public Guid SupplierId { get; set; }
}
