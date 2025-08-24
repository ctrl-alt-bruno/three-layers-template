using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Models;
using ThreeLayers.Contracts.Suppliers;
using ThreeLayers.WebApi.Mappers;

namespace ThreeLayers.WebAPI.Controllers;

public class SuppliersController(
    ISupplierRepository supplierRepository,
    ISupplierService supplierService,
    INotifier notifier,
    ILogger<SuppliersController> logger,
    ProblemDetailsFactory problemDetailsFactory)
    : CustomControllerBase(
        notifier, 
        logger,
        problemDetailsFactory)
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierResponse>>> GetAll()
    {
        List<Supplier> suppliers = (await supplierRepository.GetAllAsync()).ToList();
            
        if (suppliers.Count == 0)
            return Ok(new List<SupplierResponse>());

        return Ok(suppliers.Select(SupplierMapper.ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SupplierResponse>> GetById(Guid id)
    {
        Supplier? supplier = await supplierRepository.GetByIdAsync(id);

        if (supplier == null)
            return CreateNotFoundResult($"Supplier with id '{id}' was not found.");

        return Ok(SupplierMapper.ToResponse(supplier));
    }

    [HttpPost]
    public async Task<ActionResult<SupplierResponse>> Add(SupplierCreateRequest supplierCreateRequest)
    {
        if (!ModelState.IsValid)
            return CreateCustomActionResult(ModelState);

        Supplier supplier = SupplierMapper.ToEntity(supplierCreateRequest);
        await supplierService.AddAsync(supplier);
        SupplierResponse response = SupplierMapper.ToResponse(supplier);

        return CreateCustomActionResult(nameof(GetById), new { id = supplier.Id }, response);
    }

    // [HttpPut("{id:guid}")]
    // public async Task<IActionResult> Update(Guid id, SupplierUpdateRequest supplierUpdate)
    // {
    //     if (id != supplierUpdate.Id)
    //         Notify("error");
    //
    //     if (!ModelState.IsValid)
    //         return CustomResponse(ModelState);
    //
    //     await supplierService.UpdateAsync(SupplierMapper.ToEntity(supplierUpdate));
    //
    //     return CustomResponse(HttpStatusCode.NoContent);
    // }

    // [HttpDelete("{id:guid}")]
    // public async Task<ActionResult<SupplierResponse>> Delete(Guid id)
    // {
    //     bool deleted = await supplierService.DeleteAsync(id);
    //
    //     if (!deleted)
    //         return NotFound();
    //
    //     return CustomResponse(HttpStatusCode.NoContent);
    // }
}