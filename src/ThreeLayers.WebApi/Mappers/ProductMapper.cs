using ThreeLayers.Business.Models;
using ThreeLayers.Contracts;
using ThreeLayers.Contracts.Products;

namespace ThreeLayers.WebApi.Mappers;

public static class ProductMapper
{
    public static Product ToEntity(ProductCreateRequest dto) => new()
    {
        Id = Guid.NewGuid(),
        Name = dto.Name,
        Description = dto.Description,
        Value = dto.Value,
        SupplierId = dto.SupplierId,
        Active = false
    };

    public static Product ToEntity(ProductUpdateRequest dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Description = dto.Description,
        Value = dto.Value,
        SupplierId = dto.SupplierId,
        Active = dto.Active
    };

    public static ProductResponse ToResponse(Product entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Value = entity.Value,
        SupplierId = entity.SupplierId,
        SupplierName = entity.Supplier?.Name ?? string.Empty,
        Active = entity.Active
    };
}