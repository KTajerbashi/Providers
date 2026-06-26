using Microsoft.AspNetCore.Mvc;

namespace UnitOfWork.WebApp.Controllers;
public class ProductController : BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[]
        {
            "Product 1",
            "Product 2"
        });
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        return Ok(new
        {
            Id = id,
            Name = $"Product {id}"
        });
    }

    [HttpPost]
    public IActionResult Create(ProductDto dto)
    {
        return Ok(dto);
    }
}

public record ProductDto(string Name, decimal Price);
