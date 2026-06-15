using IdeenOgBogen.Domain.Models;
using IdeenOgBogen.Infrastructure.Data;
using IdeenOgBogen.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace IdeenOgBogen.Tests.Integration;

// The ProductRepositoryIntegrationTests class contains integration tests for the ProductRepository, which interacts with the database to perform CRUD operations on products. These tests ensure that the repository correctly retrieves, creates, and checks for products in the database, including their related entities (Category, ProductStatus, Inventory).
public class ProductRepositoryIntegrationTests
{
    private static async Task<AppDbContext> CreateContextAsync(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        return context;
    }

    // Tests that GetAllAsync returns all products from the database, including their related entities (Category, ProductStatus, Inventory)
    [Fact]
    public async Task GetAllAsync_ReturnsSeededProductsWithRelations()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        await using var context = await CreateContextAsync(connection);
        var repository = new ProductRepository(context);

        var products = await repository.GetAllAsync();

        Assert.NotEmpty(products);
        Assert.Contains(products, product => product.Name == "Clean Code");

        Assert.All(products, product =>
        {
            Assert.NotNull(product.Category);
            Assert.NotNull(product.ProductStatus);
            Assert.NotNull(product.Inventory);
        });
    }

    // Tests that GetByIdAsync returns the correct product with its related entities when the product exists in the database
    [Fact]
    public async Task CreateAsync_SavesProductAndInventoryToDatabase()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        await using var context = await CreateContextAsync(connection);
        var repository = new ProductRepository(context);

        var product = new Product
        {
            CategoryId = 2,
            ProductStatusId = 1,
            Name = "Integration Test Book",
            Description = "Created in repository integration test",
            Price = 123.45m,
            SKU = "BOOK-INTEGRATION-TEST",
            Inventory = new Inventory
            {
                Quantity = 8
            }
        };

        var createdProduct = await repository.CreateAsync(product);

        Assert.True(createdProduct.ProductId > 0);

        var productFromDatabase = await context.Products
            .Include(product => product.Inventory)
            .FirstOrDefaultAsync(product => product.SKU == "BOOK-INTEGRATION-TEST");

        Assert.NotNull(productFromDatabase);
        Assert.Equal("Integration Test Book", productFromDatabase.Name);
        Assert.Equal(8, productFromDatabase.Inventory!.Quantity);
    }

    // Tests that GetByIdAsync returns the correct product with its related entities when the product exists in the database
    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ReturnsProduct()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        await using var context = await CreateContextAsync(connection);
        var repository = new ProductRepository(context);

        var product = await repository.GetByIdAsync(1);

        Assert.NotNull(product);
        Assert.Equal("Clean Code", product.Name);
        Assert.Equal("Programmering", product.Category!.Name);
        Assert.Equal("Active", product.ProductStatus!.StatusName);
        Assert.Equal(10, product.Inventory!.Quantity);
    }

    // Tests that GetByIdAsync returns null when the product does not exist in the database
    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ReturnsNull()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        await using var context = await CreateContextAsync(connection);
        var repository = new ProductRepository(context);

        var product = await repository.GetByIdAsync(999);

        Assert.Null(product);
    }

    // Helper method to create the ProductService with a ProductRepository using the provided database connection
    [Fact]
    public async Task SKUExistsAsync_WhenSkuExists_ReturnsTrue()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        await using var context = await CreateContextAsync(connection);
        var repository = new ProductRepository(context);

        var exists = await repository.SKUExistsAsync("BOOK-CLEAN-CODE");

        Assert.True(exists);
    }
}