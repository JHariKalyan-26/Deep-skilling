using System.Collections.Concurrent;
using ProductApi.DTOs;
using ProductApi.Models;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly ConcurrentDictionary<int, Product> _products = new();
    private int _nextId = 2;

    public ProductService()
    {
        _products[1] = new Product
        {
            Id = 1,
            Name = "Laptop",
            Category = "Electronics",
            Price = 65000,
            Stock = 10
        };

        _products[2] = new Product
        {
            Id = 2,
            Name = "Keyboard",
            Category = "Accessories",
            Price = 1500,
            Stock = 25
        };
    }

    public IReadOnlyCollection<Product> GetAll() =>
        _products.Values.OrderBy(p => p.Id).ToArray();

    public Product? GetById(int id) =>
        _products.TryGetValue(id, out var product) ? product : null;

    public Product Create(ProductDto dto)
    {
        var product = new Product
        {
            Id = Interlocked.Increment(ref _nextId),
            Name = dto.Name.Trim(),
            Category = dto.Category.Trim(),
            Price = dto.Price,
            Stock = dto.Stock
        };

        _products[product.Id] = product;
        return product;
    }

    public bool Update(int id, ProductDto dto)
    {
        if (!_products.TryGetValue(id, out var product))
            return false;

        product.Name = dto.Name.Trim();
        product.Category = dto.Category.Trim();
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        return true;
    }

    public bool Delete(int id) => _products.TryRemove(id, out _);
}
