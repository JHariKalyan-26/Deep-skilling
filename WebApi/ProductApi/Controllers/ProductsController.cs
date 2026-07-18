using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController(
    IProductService service,
    ILogger<ProductsController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<IReadOnlyCollection<Product>>> GetAll()
    {
        logger.LogInformation("Retrieving all products.");

        return Ok(new ApiResponse<IReadOnlyCollection<Product>>(
            true,
            "Products retrieved successfully.",
            service.GetAll()));
    }

    [HttpGet("{id:int}")]
    public ActionResult<ApiResponse<Product>> GetById(int id)
    {
        var product = service.GetById(id);

        if (product is null)
        {
            return NotFound(new ApiResponse<Product>(
                false,
                $"Product {id} was not found.",
                null));
        }

        return Ok(new ApiResponse<Product>(
            true,
            "Product retrieved successfully.",
            product));
    }

    [HttpPost]
    public ActionResult<ApiResponse<Product>> Create(ProductDto dto)
    {
        var product = service.Create(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            new ApiResponse<Product>(
                true,
                "Product created successfully.",
                product));
    }

    [HttpPut("{id:int}")]
    public ActionResult<ApiResponse<object>> Update(int id, ProductDto dto)
    {
        if (!service.Update(id, dto))
        {
            return NotFound(new ApiResponse<object>(
                false,
                $"Product {id} was not found.",
                null));
        }

        return Ok(new ApiResponse<object>(
            true,
            "Product updated successfully.",
            null));
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ApiResponse<object>> Delete(int id)
    {
        if (!service.Delete(id))
        {
            return NotFound(new ApiResponse<object>(
                false,
                $"Product {id} was not found.",
                null));
        }

        return Ok(new ApiResponse<object>(
            true,
            "Product deleted successfully.",
            null));
    }
}
