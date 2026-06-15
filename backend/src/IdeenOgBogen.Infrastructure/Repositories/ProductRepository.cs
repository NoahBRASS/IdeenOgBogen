using IdeenOgBogen.Application.Ports;
using IdeenOgBogen.Domain.Models;
using IdeenOgBogen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdeenOgBogen.Infrastructure.Repositories;

// The ProductRepository class implements the IProductRepository interface and provides methods to interact with the products in the database
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    // Retrieves all products from the database, including related entities (Category, ProductStatus, Inventory)
    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(product => product.Category)
            .Include(product => product.ProductStatus)
            .Include(product => product.Inventory)
            .ToListAsync();
    }

    // Retrieves a product by its ID, including related entities (Category, ProductStatus, Inventory)
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(product => product.Category)
            .Include(product => product.ProductStatus)
            .Include(product => product.Inventory)
            .FirstOrDefaultAsync(product => product.ProductId == id);
    }

    // Creates a new product in the database and returns the created product with its generated ID
    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var createdProduct = await GetByIdAsync(product.ProductId);

        return createdProduct ?? product;
    }

    // Checks if a product with the given SKU already exists in the database (case-insensitive)
    public async Task<bool> SKUExistsAsync(string sku)
    {
        return await _context.Products
            .AnyAsync(product => product.SKU.ToLower() == sku.ToLower());
    }
}