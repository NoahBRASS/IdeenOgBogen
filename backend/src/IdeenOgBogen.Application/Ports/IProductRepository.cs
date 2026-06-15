using IdeenOgBogen.Domain.Models;

namespace IdeenOgBogen.Application.Ports;

// The IProductRepository interface defines the contract for the ProductRepository, specifying the methods that must be implemented to interact with the products in the database, including retrieving all products, retrieving a product by ID, creating a new product, and checking for SKU existence.
public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<bool> SKUExistsAsync(string sku);
}