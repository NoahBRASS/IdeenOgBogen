using IdeenOgBogen.Application.DTOs.Products;
using IdeenOgBogen.Application.Ports;
using IdeenOgBogen.Domain.Models;

namespace IdeenOgBogen.Application.Services;

// The ProductService class implements the IProductService interface and provides methods to manage products, including retrieving all products, retrieving a product by ID, and creating a new product. It uses the IProductRepository to interact with the data layer.
public class ProductService : IProductService
{
    // The IProductRepository is injected into the ProductService through the constructor, allowing the service to perform data operations related to products.
    private readonly IProductRepository _productRepository;

    // Constructor that initializes the ProductService with an instance of IProductRepository
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // Retrieves all products from the repository, maps them to ProductResponseDto objects, and returns the list of products.
    public async Task<List<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products.Select(MapToProductResponse).ToList();
    }

    // Retrieves a product by its ID from the repository. If the product exists, it maps it to a ProductResponseDto and returns it; otherwise, it returns null.
    public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return null;
        }

        return MapToProductResponse(product);
    }

    // Creates a new product based on the provided CreateProductDto. It validates the input, checks for SKU uniqueness, creates the product in the repository, and returns the created product as a ProductResponseDto.
    public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Product name is required.", nameof(dto.Name));
        }

        if (string.IsNullOrWhiteSpace(dto.SKU))
        {
            throw new ArgumentException("SKU is required.", nameof(dto.SKU));
        }

        if (dto.Price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.Price), "Price cannot be negative.");
        }

        if (dto.Quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.Quantity), "Quantity cannot be negative.");
        }

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
            Inventory = new Inventory
            {
                Quantity = dto.Quantity
            }
        };

        var createdProduct = await _productRepository.CreateAsync(product);

        return MapToProductResponse(createdProduct);
    }

    // Maps a Product domain model to a ProductResponseDto, which is used to transfer product data in a format suitable for API responses.
    private static ProductResponseDto MapToProductResponse(Product product)
    {
        return new ProductResponseDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            SKU = product.SKU,
            CategoryName = product.Category?.Name ?? "Unknown",
            StatusName = product.ProductStatus?.StatusName ?? "Unknown",
            Quantity = product.Inventory?.Quantity ?? 0
        };
    }
}