using ProductApi.DTOs;
using ProductApi.Models;

namespace ProductApi.Services;

public interface IProductService
{
    IReadOnlyCollection<Product> GetAll();
    Product? GetById(int id);
    Product Create(ProductDto dto);
    bool Update(int id, ProductDto dto);
    bool Delete(int id);
}
