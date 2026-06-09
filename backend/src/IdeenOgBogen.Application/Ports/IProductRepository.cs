using IdeenOgBogen.Domain.Models;

namespace IdeenOgBogen.Application.Ports;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<bool> SKUExistsAsync(string sku);
}