using IdeenOgBogen.Application.DTOs.Products;
using IdeenOgBogen.Application.Ports;
using IdeenOgBogen.Application.Services;
using IdeenOgBogen.Domain.Models;

namespace IdeenOgBogen.Tests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        // Arrange
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        // Act
        var result = await service.GetAllProductsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, product => product.Name == "Clean Code");
        Assert.Contains(result, product => product.Name == "The Hobbit");
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductExists_ReturnsProduct()
    {
        // Arrange
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        // Act
        var result = await service.GetProductByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ProductId);
        Assert.Equal("Clean Code", result.Name);
        Assert.Equal("Programmering", result.CategoryName);
        Assert.Equal("Active", result.StatusName);
        Assert.Equal(10, result.Quantity);
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        // Act
        var result = await service.GetProductByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateProductAsync_WhenSkuDoesNotExist_CreatesProduct()
    {
        // Arrange
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var dto = new CreateProductDto
        {
            CategoryId = 2,
            ProductStatusId = 1,
            Name = "Refactoring",
            Description = "Bog om at forbedre eksisterende kode",
            Price = 249.95m,
            SKU = "BOOK-REFACTORING",
            Quantity = 7
        };

        // Act
        var result = await service.CreateProductAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Refactoring", result.Name);
        Assert.Equal("BOOK-REFACTORING", result.SKU);
        Assert.Equal("Programmering", result.CategoryName);
        Assert.Equal("Active", result.StatusName);
        Assert.Equal(7, result.Quantity);
    }

    [Fact]
    public async Task CreateProductAsync_WhenSkuAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var dto = new CreateProductDto
        {
            CategoryId = 2,
            ProductStatusId = 1,
            Name = "Duplicate Clean Code",
            Description = "Duplicate test",
            Price = 299.95m,
            SKU = "BOOK-CLEAN-CODE",
            Quantity = 3
        };

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateProductAsync(dto));
    }

    private sealed class FakeProductRepository : IProductRepository
    {
        private readonly List<Product> _products =
        [
            new Product
            {
                ProductId = 1,
                CategoryId = 2,
                ProductStatusId = 1,
                Name = "Clean Code",
                Description = "Bog om god softwareudvikling",
                Price = 299.95m,
                SKU = "BOOK-CLEAN-CODE",
                Category = new Category
                {
                    CategoryId = 2,
                    Name = "Programmering"
                },
                ProductStatus = new ProductStatus
                {
                    ProductStatusId = 1,
                    StatusName = "Active",
                    IsSellable = true
                },
                Inventory = new Inventory
                {
                    ProductId = 1,
                    Quantity = 10
                }
            },
            new Product
            {
                ProductId = 2,
                CategoryId = 3,
                ProductStatusId = 1,
                Name = "The Hobbit",
                Description = "Fantasyklassiker",
                Price = 149.95m,
                SKU = "BOOK-HOBBIT",
                Category = new Category
                {
                    CategoryId = 3,
                    Name = "Fantasy"
                },
                ProductStatus = new ProductStatus
                {
                    ProductStatusId = 1,
                    StatusName = "Active",
                    IsSellable = true
                },
                Inventory = new Inventory
                {
                    ProductId = 2,
                    Quantity = 5
                }
            }
        ];

        public Task<List<Product>> GetAllAsync()
        {
            return Task.FromResult(_products);
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(product => product.ProductId == id);

            return Task.FromResult(product);
        }

        public Task<Product> CreateAsync(Product product)
        {
            product.ProductId = _products.Max(existingProduct => existingProduct.ProductId) + 1;

            product.Category = new Category
            {
                CategoryId = product.CategoryId,
                Name = product.CategoryId == 2 ? "Programmering" : "Unknown"
            };

            product.ProductStatus = new ProductStatus
            {
                ProductStatusId = product.ProductStatusId,
                StatusName = product.ProductStatusId == 1 ? "Active" : "Unknown",
                IsSellable = product.ProductStatusId == 1
            };

            product.Inventory ??= new Inventory();
            product.Inventory.ProductId = product.ProductId;
            product.Inventory.Product = product;

            _products.Add(product);

            return Task.FromResult(product);
        }

        public Task<bool> SKUExistsAsync(string sku)
        {
            var exists = _products.Any(product =>
                product.SKU.Equals(sku, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(exists);
        }
    }
}