using ThreeLayers.Business.Models;
using ThreeLayers.Contracts.Suppliers.Addresses;

namespace ThreeLayers.WebApi.Mappers;

public static class AddressMapper
{
    public static Address ToEntity(AddressCreateRequest dto) => new()
    {
        Id = Guid.NewGuid(),
        Street = dto.Street,
        Number = dto.Number,
        Complement = dto.Complement,
        PostalCode = dto.PostalCode,
        Region = dto.Region,
        City = dto.City,
        State = dto.State
    };

    // public static Address ToEntity(AddressUpdateRequest dto) => new()
    // {
    //     Id = dto.Id,
    //     Name = dto.Name,
    //     Description = dto.Description,
    //     Value = dto.Value,
    //     SupplierId = dto.SupplierId,
    //     Active = dto.Active
    // };

    public static AddressResponse ToResponse(Address entity) => new()
    {
        Id = Guid.NewGuid(),
        Street = entity.Street,
        Number = entity.Number,
        Complement = entity.Complement,
        PostalCode = entity.PostalCode,
        Region = entity.Region,
        City = entity.City,
        State = entity.State
    };
}