using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Models;
using ThreeLayers.Business.Models.Validation;

namespace ThreeLayers.Business.Services;

public class ProductService(IProductRepository productRepository, INotifier notifier)
    : BaseService(notifier), IProductService
{
    public async Task<bool> AddAsync(Product product)
    {
        if (!Validate(new ProductValidation(), product))
            return false;

        // Check if supplier exists
        if (product.SupplierId == Guid.Empty)
        {
            NotifyBusinessRule("Supplier ID is required");
            return false;
        }

        try
        {
            await productRepository.AddAsync(product);
            return true;
        }
        catch (Exception ex)
        {
            Notify($"Error adding product: {ex.Message}", Notifications.NotificationType.BadRequest);
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        if (!Validate(new ProductValidation(), product))
            return false;

        Product? existingProduct = await productRepository.GetByIdAsync(product.Id);

        if (existingProduct == null)
        {
            NotifyNotFound("Product");
            return false;
        }

        // Update properties
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Value = product.Value;
        existingProduct.Active = product.Active;
        existingProduct.SupplierId = product.SupplierId;

        try
        {
            await productRepository.UpdateAsync(existingProduct);
            return true;
        }
        catch (Exception ex)
        {
            Notify($"Error updating product: {ex.Message}", Notifications.NotificationType.BadRequest);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            Notify("Product ID is required", Notifications.NotificationType.BadRequest);
            return false;
        }

        Product? productToDelete = await productRepository.GetByIdAsync(productId);
            
        if (productToDelete == null)
        {
            NotifyNotFound("Product");
            return false;
        }

        try
        {
            await productRepository.DeleteAsync(productId);
            return true;
        }
        catch (Exception ex)
        {
            Notify($"Error deleting product: {ex.Message}", Notifications.NotificationType.BadRequest);
            return false;
        }
    }

    public void Dispose()
    {
        productRepository.Dispose();
    }
}