using IdeenOgBogen.Application.DTOs.Products;

namespace IdeenOgBogen.Application.Services;

// The IProductService interface defines the contract for the ProductService, specifying the methods that must be implemented to manage products, including retrieving all products, retrieving a product by ID, and creating a new product.
public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllProductsAsync();
    Task<ProductResponseDto?> GetProductByIdAsync(int id);
    Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto);
}