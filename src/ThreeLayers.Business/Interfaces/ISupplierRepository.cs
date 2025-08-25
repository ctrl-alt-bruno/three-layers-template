using ThreeLayers.Business.Models;

namespace ThreeLayers.Business.Interfaces;

public interface ISupplierRepository : ICommandRepository<Supplier>, IQueryRepository<Supplier>
{
    Task<Supplier?> GetSupplierAndProductsAsync(Guid supplierId);
    Task<Supplier?> GetSupplierAndAddressAsync(Guid supplierId);
    Task<Supplier?> GetSupplierAndProductsAndAddressAsync(Guid supplierId);
    Task<Address?> GetSupplierAddressAsync(Guid supplierId);
    Task DeleteSupplierAddress(Address address);
}
