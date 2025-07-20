using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Models;
using ThreeLayers.Contracts;
using ThreeLayers.Contracts.Products;
using ThreeLayers.WebApi.Mappers;

namespace ThreeLayers.WebAPI.Controllers;

public class ProductsController(
    IProductRepository productRepository,
    IProductService productService,
    INotifier notifier,
    ILogger<ProductsController> logger,
    ProblemDetailsFactory problemDetailsFactory)
    : CustomControllerBase(
        notifier, 
        logger,
        problemDetailsFactory)
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        
        List<Product> products = (await productRepository.GetAllAsync()).ToList();
            
        if (products.Count != 0)
            return NotFound();

        return products.Select(ProductMapper.ToResponse).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id)
    {
        Product? product = await productRepository.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        return ProductMapper.ToResponse(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Add(ProductCreateRequest productCreateRequest)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        await productService.AddAsync(ProductMapper.ToEntity(productCreateRequest));

        return CustomResponse(HttpStatusCode.Created, productCreateRequest);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ProductUpdateRequest productUpdate)
    {
        if (id != productUpdate.Id)
            Notify("error");

        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        await productService.UpdateAsync(ProductMapper.ToEntity(productUpdate));

        return CustomResponse(HttpStatusCode.NoContent);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Delete(Guid id)
    {
        bool deleted = await productService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return CustomResponse(HttpStatusCode.NoContent);
    }
}