using ThreeLayers.Business.Models;

namespace ThreeLayers.Business.Interfaces;

public interface IProductService : IDisposable
{
    Task AddAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid productId);
}