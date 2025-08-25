using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Models;
using ThreeLayers.Contracts.Products;
using ThreeLayers.WebApi.Mappers;

namespace ThreeLayers.WebAPI.Controllers;

public class ProductsController(
    IProductRepository productRepository,
    IProductService productService,
    INotifier notifier,
    ILogger<ProductsController> logger,
    ProblemDetailsFactory problemDetailsFactory
) : CustomControllerBase(notifier, logger, problemDetailsFactory)
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        List<Product> products = (await productRepository.GetAllAsync()).ToList();

        if (products.Count == 0)
        {
            NotifyNotFound("No products found");
            return CreateCustomActionResult();
        }

        return Ok(products.Select(ProductMapper.ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            Notify("Product ID is required");
            return CreateCustomActionResult();
        }

        Product? product = await productRepository.GetByIdAsync(id);

        if (product == null)
        {
            NotifyNotFound("Product not found");
            return CreateCustomActionResult();
        }

        return Ok(ProductMapper.ToResponse(product));
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Add(ProductCreateRequest productCreateRequest)
    {
        if (!ModelState.IsValid)
            return CreateCustomActionResult(ModelState);

        Product product = ProductMapper.ToEntity(productCreateRequest);
        bool success = await productService.AddAsync(product);

        if (!success)
            return CreateCustomActionResult();

        ProductResponse response = ProductMapper.ToResponse(product);
        return CreateCustomActionResult(nameof(GetById), new { id = product.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ProductUpdateRequest productUpdate)
    {
        if (id == Guid.Empty)
        {
            Notify("Product ID is required");
            return CreateCustomActionResult();
        }

        if (id != productUpdate.Id)
        {
            Notify("Product ID in URL does not match ID in request body");
            return CreateCustomActionResult();
        }

        if (!ModelState.IsValid)
            return CreateCustomActionResult(ModelState);

        bool success = await productService.UpdateAsync(ProductMapper.ToEntity(productUpdate));

        if (!success)
            return CreateCustomActionResult();

        return CreateCustomActionResult(HttpStatusCode.NoContent);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            Notify("Product ID is required");
            return CreateCustomActionResult();
        }

        bool success = await productService.DeleteAsync(id);

        if (!success)
            return CreateCustomActionResult();

        return CreateCustomActionResult(HttpStatusCode.NoContent);
    }
}
