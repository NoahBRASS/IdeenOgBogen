using IdeenOgBogen.Application.DTOs.Products;
using IdeenOgBogen.Application.Services;
using Xunit;

namespace IdeenOgBogen.Tests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateProductAsync_ShouldReturnProductResponse_WhenProductIsValid()
    {
        // Arrange
        var service = new ProductService();

        var dto = new CreateProductDto
        {
            Name = "Harry Potter",
            Description = "Fantasy book",
            Price = 199,
            CategoryId = 1
        };

        // Act
        var result = await service.CreateProductAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Harry Potter", result.Name);
        Assert.Equal(199, result.Price);
    }
}