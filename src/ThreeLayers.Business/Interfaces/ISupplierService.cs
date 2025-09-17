using ThreeLayers.Business.Models;

namespace ThreeLayers.Business.Interfaces;

public interface ISupplierService : IDisposable
{
    Task<bool> AddAsync(Supplier supplier);
    Task<bool> UpdateAsync(Supplier supplier);
    Task<bool> DeleteAsync(Guid supplierId);
}
