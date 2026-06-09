using IdeenOgBogen.Application.Ports;
using IdeenOgBogen.Domain.Models;
using IdeenOgBogen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdeenOgBogen.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductStatus)
            .Include(p => p.Inventory)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductStatus)
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> SKUExistsAsync(string sku)
    {
        return await _context.Products.AnyAsync(p => p.SKU == sku);
    }
}