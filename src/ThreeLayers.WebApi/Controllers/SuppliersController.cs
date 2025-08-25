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
        {
            NotifyNotFound("No suppliers found");
            return CreateCustomActionResult();
        }

        return Ok(suppliers.Select(SupplierMapper.ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SupplierResponse>> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            Notify("Supplier ID is required");
            return CreateCustomActionResult();
        }

        Supplier? supplier = await supplierRepository.GetByIdAsync(id);

        if (supplier == null)
        {
            NotifyNotFound("Supplier not found");
            return CreateCustomActionResult();
        }

        return Ok(SupplierMapper.ToResponse(supplier));
    }

    [HttpPost]
    public async Task<ActionResult<SupplierResponse>> Add(SupplierCreateRequest supplierCreateRequest)
    {
        if (!ModelState.IsValid)
            return CreateCustomActionResult(ModelState);

        Supplier supplier = SupplierMapper.ToEntity(supplierCreateRequest);
        bool success = await supplierService.AddAsync(supplier);

        if (!success)
            return CreateCustomActionResult();

        SupplierResponse response = SupplierMapper.ToResponse(supplier);
        return CreateCustomActionResult(nameof(GetById), new { id = supplier.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, SupplierUpdateRequest supplierUpdate)
    {
        if (id == Guid.Empty)
        {
            Notify("Supplier ID is required");
            return CreateCustomActionResult();
        }

        if (id != supplierUpdate.Id)
        {
            Notify("Supplier ID in URL does not match ID in request body");
            return CreateCustomActionResult();
        }

        if (!ModelState.IsValid)
            return CreateCustomActionResult(ModelState);

        bool success = await supplierService.UpdateAsync(SupplierMapper.ToEntity(supplierUpdate));

        if (!success)
            return CreateCustomActionResult();

        return CreateCustomActionResult(HttpStatusCode.NoContent);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            Notify("Supplier ID is required");
            return CreateCustomActionResult();
        }

        bool success = await supplierService.DeleteAsync(id);

        if (!success)
            return CreateCustomActionResult();

        return CreateCustomActionResult(HttpStatusCode.NoContent);
    }
}