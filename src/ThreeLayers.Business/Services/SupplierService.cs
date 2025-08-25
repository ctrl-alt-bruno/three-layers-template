using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Models;
using ThreeLayers.Business.Models.Validation;

namespace ThreeLayers.Business.Services;

public class SupplierService(ISupplierRepository supplierRepository, INotifier notifier)
    : BaseService(notifier), ISupplierService
{
    public async Task<bool> AddAsync(Supplier supplier)
    {
        if (!Validate(new SupplierValidation(), supplier) || 
            !Validate(new AddressValidation(), supplier.Address))
            return false;

        if (supplierRepository.FindAsync(s => s.Document == supplier.Document).Result.Any())
        {
            NotifyConflict("A supplier with this document already exists");
            return false;
        }

        try
        {
            await supplierRepository.AddAsync(supplier);
            return true;
        }
        catch (Exception ex)
        {
            Notify($"Error adding supplier: {ex.Message}", Notifications.NotificationType.BadRequest);
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Supplier supplier)
    {
        if (!Validate(new SupplierValidation(), supplier))
            return false;

        Supplier? existingSupplier = await supplierRepository.GetByIdAsync(supplier.Id);

        if (existingSupplier == null)
        {
            NotifyNotFound("Supplier");
            return false;
        }

        if (supplierRepository.FindAsync(s => s.Document == supplier.Document && s.Id != supplier.Id).Result.Any())
        {
            NotifyConflict("A supplier with this document already exists");
            return false;
        }

        try
        {
            await supplierRepository.UpdateAsync(supplier);
            return true;
        }
        catch (Exception ex)
        {
            Notify($"Error updating supplier: {ex.Message}", Notifications.NotificationType.BadRequest);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid supplierId)
    {
        if (supplierId == Guid.Empty)
        {
            Notify("Supplier ID is required", Notifications.NotificationType.BadRequest);
            return false;
        }

        Supplier? supplier = await supplierRepository.GetSupplierAndProductsAndAddressAsync(supplierId);

        if (supplier == null)
        {
            NotifyNotFound("Supplier");
            return false;
        }

        if (supplier.Products.Any())
        {
            NotifyBusinessRule("Cannot delete supplier with associated products");
            return false;
        }

        try
        {
            Address? address = await supplierRepository.GetSupplierAddressAsync(supplierId);

            if (address != null)
            {
                await supplierRepository.DeleteSupplierAddress(address);
            }

            await supplierRepository.DeleteAsync(supplierId);
            return true;
        }
        catch (Exception ex)
        {
            Notify($"Error deleting supplier: {ex.Message}", Notifications.NotificationType.BadRequest);
            return false;
        }
    }

    public void Dispose()
    {
        supplierRepository.Dispose();
    }
}