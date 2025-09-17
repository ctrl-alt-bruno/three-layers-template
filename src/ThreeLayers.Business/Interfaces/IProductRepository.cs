using ThreeLayers.Business.Models;

namespace ThreeLayers.Business.Interfaces;

public interface IProductRepository : ICommandRepository<Product>, IQueryRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsBySupplierIdAsync(Guid supplierId);
    Task<IEnumerable<Product>> GetAllProductsAndSuppliersAsync();
    Task<Product?> GetProductAndSupplierAsync(Guid id);
}
