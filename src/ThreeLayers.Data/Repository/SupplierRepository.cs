using Microsoft.EntityFrameworkCore;
using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Models;
using ThreeLayers.Data.Context;

namespace ThreeLayers.Data.Repository;

public class SupplierRepository(MyDbContext dbContext) : Repository<Supplier>(dbContext), ISupplierRepository
{
    public async Task<Supplier?> GetSupplierAndAddressAsync(Guid supplierId)
    {
        return await DbContext.Suppliers
            .AsNoTracking()
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Id == supplierId);
    }

    public async Task<Supplier?> GetSupplierAndProductsAndAddressAsync(Guid supplierId)
    {
        return await DbContext.Suppliers
            .AsNoTracking()
            .Include(x => x.Products)
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Id == supplierId);
    }

    public async Task<Address?> GetSupplierAddressAsync(Guid supplierId)
    {
        return await DbContext.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SupplierId == supplierId);
    }

    public async Task DeleteSupplierAddress(Address address)
    {
        DbContext.Addresses.Remove(address);
        await SaveChangesAsync();
    }
}