using System.ComponentModel.DataAnnotations;
using ThreeLayers.Contracts.Products;

namespace ThreeLayers.Contracts.Suppliers;

public class SupplierRequest
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

	public SupplierRequest()
	{
		Name = Document = String.Empty;
	}
}