using ThreeLayers.Business.Models;
using ThreeLayers.Contracts.Products;
using ThreeLayers.Contracts.Suppliers;

namespace ThreeLayers.WebApi.Mappers;

public static class SupplierMapper
{
    public static Supplier ToEntity(SupplierCreateRequest dto) =>
        new Supplier()
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Document = dto.Document,
            SupplierType = (SupplierTypes)dto.SupplierType,
            Active = false,
            Address = AddressMapper.ToEntity(dto.Address!),
        };

    public static Supplier ToEntity(SupplierUpdateRequest dto) =>
        new Supplier()
        {
            Id = dto.Id,
            Name = dto.Name,
            Document = dto.Document,
            SupplierType = (SupplierTypes)dto.SupplierType,
            Active = dto.Active,
            Address = AddressMapper.ToEntity(dto.Address!),
        };

    public static SupplierResponse ToResponse(Supplier entity) =>
        new SupplierResponse()
        {
            Id = entity.Id,
            Name = entity.Name,
            Active = entity.Active,
            Document = entity.Document,
            SupplierType = (short)entity.SupplierType,
            Address = entity.Address != null ? AddressMapper.ToResponse(entity.Address) : null,
            Products = entity.Products.Any()
                ? (from p in entity.Products select ProductMapper.ToResponse(p))
                : null,
        };
}
