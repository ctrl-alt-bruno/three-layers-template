using System.Text.Json.Serialization;

namespace ThreeLayers.Contracts.Products;

public class ProductResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("creation_date")]
    public DateTime CreationDate { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("supplier_id")]
    public Guid SupplierId { get; set; }

    [JsonPropertyName("supplier_name")]
    public required string SupplierName { get; set; }
}