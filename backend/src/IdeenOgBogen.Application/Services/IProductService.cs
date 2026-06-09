using IdeenOgBogen.Application.DTOs.Products;

namespace IdeenOgBogen.Application.Services;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllProductsAsync();
    Task<ProductResponseDto?> GetProductByIdAsync(int id);
    Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto);
}