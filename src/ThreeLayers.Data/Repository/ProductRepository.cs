using Microsoft.EntityFrameworkCore;
using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Models;
using ThreeLayers.Data.Context;

namespace ThreeLayers.Data.Repository;

public class ProductRepository(MyDbContext dbContext)
    : Repository<Product>(dbContext),
        IProductRepository
{
    public async Task<IEnumerable<Product>> GetProductsBySupplierIdAsync(Guid supplierId)
    {
        return await FindAsync(x => x.SupplierId == supplierId);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAndSuppliersAsync()
    {
        return await DbContext
            .Products.AsNoTracking()
            .Include(x => x.Supplier)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetProductAndSupplierAsync(Guid id)
    {
        return await DbContext
            .Products.AsNoTracking()
            .Include(x => x.Supplier)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
