using ThreeLayers.Business.Models;
using ThreeLayers.Contracts;
using ThreeLayers.Contracts.Suppliers;

namespace ThreeLayers.WebApi.Mappers;

public static class SupplierMapper
{
    public static Supplier ToEntity(SupplierCreateRequest dto) => new()
    {
        Id = Guid.NewGuid(),
        Name = dto.Name,
        Document = dto.Document,
        SupplierType = (SupplierTypes)dto.SupplierType,
        Active = false,
        Address = AddressMapper.ToEntity(dto.Address!)
    };

    // public static Supplier ToEntity(SupplierUpdateRequest dto) => new()
    // {
    //     Id = dto.Id,
    //     Name = dto.Name,
    //     Description = dto.Description,
    //     Value = dto.Value,
    //     SupplierId = dto.SupplierId,
    //     Active = dto.Active
    // };

    public static SupplierResponse ToResponse(Supplier entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Document = entity.Document,
        SupplierType = (int)entity.SupplierType,
        Active = entity.Active,
        Address = AddressMapper.ToResponse(entity.Address!)
    };
}