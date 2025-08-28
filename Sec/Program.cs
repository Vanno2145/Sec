using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private static readonly List<Product> _products = new List<Product>
    {
        new Product { Id = 1, Name = "Ноутбук Dell XPS", Description = "Мощный и легкий ноутбук", Price = 1500.00m },
        new Product { Id = 2, Name = "Клавиатура Logitech", Description = "Механическая клавиатура с подсветкой", Price = 120.50m },
        new Product { Id = 3, Name = "Мышь Razer DeathAdder", Description = "Игровая мышь с высокой точностью", Price = 75.00m }
    };

    private static int _nextId = _products.Count + 1;

    [HttpGet("index")]
    public ActionResult<IEnumerable<Product>> Index()
    {
        return Ok(_products);
    }
    [HttpPost("create")]
    public ActionResult<Product> Create([FromBody] Product newProduct)
    {
        if (newProduct == null || string.IsNullOrEmpty(newProduct.Name))
        {
            return BadRequest("Некорректные данные товара.");
        }
        newProduct.Id = _nextId++;
        _products.Add(newProduct);
        return CreatedAtAction(nameof(Details), new { id = newProduct.Id }, newProduct);
    }


    [HttpGet("search")]
    public ActionResult<IEnumerable<Product>> Search([FromQuery] string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return BadRequest("Ключевое слово не может быть пустым.");
        }
        
        var foundProducts = _products.Where(p => 
            p.Name.Contains(keyword, System.StringComparison.OrdinalIgnoreCase) || 
            p.Description.Contains(keyword, System.StringComparison.OrdinalIgnoreCase)).ToList();

        return Ok(foundProducts);
    }

    [HttpGet("details/{id}")]
    public ActionResult<Product> Details(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound("Товар не найден.");
        }
        return Ok(product);
    }
    [HttpPost("delete/{id}")]
    public ActionResult<Product> Delete(int id)
    {
        var productToRemove = _products.FirstOrDefault(p => p.Id == id);
        if (productToRemove == null)
        {
            return NotFound("Товар не найден.");
        }
        _products.Remove(productToRemove);
        return Ok(productToRemove);
    }
}
