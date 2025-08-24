using ThreeLayers.Business.Exceptions;
using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Models;
using ThreeLayers.Business.Models.Validation;

namespace ThreeLayers.Business.Services;

public class ProductService(IProductRepository productRepository, INotifier notifier)
    : BaseService(notifier), IProductService
{
    public async Task AddAsync(Product product)
    {
        if (!Validate(new ProductValidation(), product))
            return;

        await productRepository.AddAsync(product);
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        if (!Validate(new ProductValidation(), product))
            return false;

        Product? productToUpdate = await productRepository.GetByIdAsync(product.Id);

        if (productToUpdate == null)
            throw new EntityNotFoundException("Product", product.Id);
			 
        productToUpdate.Name = product.Name;
        productToUpdate.Description = product.Description;
        productToUpdate.Value = product.Value;
        productToUpdate.Active = product.Active;

        await productRepository.UpdateAsync(product);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid productId)
    {
        Product? productToDelete = await productRepository.GetByIdAsync(productId);
            
        if (productToDelete == null)
            throw new EntityNotFoundException("Product", productId);
            
        await productRepository.DeleteAsync(productId);
            
        return true;
    }

    public void Dispose()
    {
        productRepository.Dispose();
    }
}