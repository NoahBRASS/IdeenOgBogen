using IdeenOgBogen.Application.DTOs.Products;
using IdeenOgBogen.Application.Services;
using IdeenOgBogen.Infrastructure.Data;
using IdeenOgBogen.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace IdeenOgBogen.Tests.Integration;

// The ProductServiceIntegrationTests class contains integration tests for the ProductService, which manages products and interacts with the 
// ProductRepository to perform operations on the database. These tests ensure that the service correctly retrieves, creates, and validates 
// products in the database, including their related entities (Category, ProductStatus, Inventory).
public class ProductServiceIntegrationTests
{
    private static async Task<(AppDbContext Context, ProductService Service)> CreateServiceAsync(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var repository = new ProductRepository(context);
        var service = new ProductService(repository);

        return (context, service);
    }

    // Tests that GetAllProductsAsync returns all products from the database, including their related entities (Category, ProductStatus, Inventory)
    [Fact]
    public async Task GetProductByIdAsync_WhenProductExists_ReturnsProduct()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var setup = await CreateServiceAsync(connection);
        await using var context = setup.Context;
        var service = setup.Service;

        var product = await service.GetProductByIdAsync(1);

        Assert.NotNull(product);
        Assert.Equal("Clean Code", product.Name);
        Assert.Equal("Programmering", product.CategoryName);
        Assert.Equal("Active", product.StatusName);
        Assert.Equal(10, product.Quantity);
    }

    // Tests that GetProductByIdAsync returns null when the product does not exist in the database
    [Fact]
    public async Task CreateProductAsync_SavesProductThroughServiceAndDatabase()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var setup = await CreateServiceAsync(connection);
        await using var context = setup.Context;
        var service = setup.Service;

        var dto = new CreateProductDto
        {
            CategoryId = 2,
            ProductStatusId = 1,
            Name = "Service Integration Book",
            Description = "Created through service integration test",
            Price = 199.95m,
            SKU = "BOOK-SERVICE-INTEGRATION",
            Quantity = 4
        };

        var createdProduct = await service.CreateProductAsync(dto);

        Assert.True(createdProduct.ProductId > 0);
        Assert.Equal("Service Integration Book", createdProduct.Name);
        Assert.Equal("Programmering", createdProduct.CategoryName);
        Assert.Equal("Active", createdProduct.StatusName);
        Assert.Equal(4, createdProduct.Quantity);

        var productExistsInDatabase = await context.Products
            .AnyAsync(p => p.SKU == "BOOK-SERVICE-INTEGRATION");

        Assert.True(productExistsInDatabase);
    }

    // Tests that GetAllProductsAsync returns all products from the database, including their related entities (Category, ProductStatus, Inventory)
    [Fact]
    public async Task CreateProductAsync_WhenSkuAlreadyExists_ThrowsInvalidOperationException()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var setup = await CreateServiceAsync(connection);
        await using var context = setup.Context;
        var service = setup.Service;

        var dto = new CreateProductDto
        {
            CategoryId = 2,
            ProductStatusId = 1,
            Name = "Duplicate Book",
            Description = "Duplicate SKU test",
            Price = 99.95m,
            SKU = "BOOK-CLEAN-CODE",
            Quantity = 1
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateProductAsync(dto));
    }

    // Tests that GetProductByIdAsync returns null when the product does not exist in the database
    [Fact]
    public async Task CreateProductAsync_WhenQuantityIsNegative_ThrowsArgumentOutOfRangeException()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var setup = await CreateServiceAsync(connection);
        await using var context = setup.Context;
        var service = setup.Service;

        var dto = new CreateProductDto
        {
            CategoryId = 2,
            ProductStatusId = 1,
            Name = "Invalid Quantity Book",
            Description = "Invalid quantity test",
            Price = 99.95m,
            SKU = "BOOK-INVALID-QUANTITY",
            Quantity = -1
        };

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            service.CreateProductAsync(dto));
    }
}