using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ThreeLayers.Contracts.Products;
using ThreeLayers.Contracts.Suppliers.Addresses;

namespace ThreeLayers.Contracts.Suppliers;

public class SupplierResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("document")]
    public string Document { get; set; }

    [JsonPropertyName("type")]
    public short SupplierType { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("address")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AddressResponse? Address { get; set; }

    [JsonPropertyName("products")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ProductResponse>? Products { get; set; }

    public SupplierResponse()
    {
        Name = Document = String.Empty;
        Products = new List<ProductResponse>();
    }
}
