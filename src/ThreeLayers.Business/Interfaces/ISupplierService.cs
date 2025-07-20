using ThreeLayers.Business.Models;

namespace ThreeLayers.Business.Interfaces;

public interface ISupplierService : IDisposable
{
    Task AddAsync(Supplier supplier);
    Task UpdateAsync(Supplier supplier);
    Task DeleteAsync(Guid supplierId);
}