using System.ComponentModel.DataAnnotations;
using ThreeLayers.Contracts.Products;
using ThreeLayers.Contracts.Suppliers.Addresses;

namespace ThreeLayers.Contracts.Suppliers;

public class SupplierResponse
{
	public Guid Id { get; set; }

	[Required]
	[StringLength(100)]
	public string Name { get; set; }

	[Required]
	[StringLength(20)]
	public string Document { get; set; }

	public int SupplierType { get; set; }

	public bool Active { get; set; }

	public AddressResponse? Address { get; set; }

	public IEnumerable<ProductResponse> Products { get; set; } = new List<ProductResponse>();

	public SupplierResponse()
	{
		Name = Document = String.Empty;
	}
}