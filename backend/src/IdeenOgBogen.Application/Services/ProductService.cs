using IdeenOgBogen.Application.DTOs.Products;
using IdeenOgBogen.Application.Ports;
using IdeenOgBogen.Domain.Models;

namespace IdeenOgBogen.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products
            .Select(MapToResponseDto)
            .ToList();
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return null;
        }

        return MapToResponseDto(product);
    }

    public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
    {
        var sku = dto.SKU.Trim();

        if (await _productRepository.SKUExistsAsync(sku))
        {
            throw new InvalidOperationException($"A product with SKU '{sku}' already exists.");
        }

        var product = new Product
        {
            CategoryId = dto.CategoryId,
            ProductStatusId = dto.ProductStatusId,
            Name = dto.Name.Trim(),
            Description = dto.Description,
            Price = dto.Price,
            SKU = sku,
            CreatedAt = DateTime.UtcNow,
            Inventory = new Inventory
            {
                Quantity = dto.Quantity,
                LastUpdated = DateTime.UtcNow
            }
        };

        var createdProduct = await _productRepository.CreateAsync(product);

        var productWithRelations = await _productRepository.GetByIdAsync(createdProduct.ProductId);

        return MapToResponseDto(productWithRelations ?? createdProduct);
    }

    private static ProductResponseDto MapToResponseDto(Product product)
    {
        return new ProductResponseDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            SKU = product.SKU,
            CategoryName = product.Category?.Name ?? string.Empty,
            StatusName = product.ProductStatus?.StatusName ?? string.Empty,
            Quantity = product.Inventory?.Quantity ?? 0
        };
    }
}