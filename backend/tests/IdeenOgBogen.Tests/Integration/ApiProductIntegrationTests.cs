using System.Net;
using System.Net.Http.Json;
using IdeenOgBogen.Application.DTOs.Products;
using IdeenOgBogen.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IdeenOgBogen.Tests.Integration;

// The ApiProductIntegrationTests class contains integration tests for the Products API endpoints, which interact with a real MySQL database. 
// These tests ensure that the API correctly retrieves, creates, and validates products in the database, 
// including their related entities (Category, ProductStatus, Inventory). The tests use a custom WebApplicationFactory to 
// configure the application to use a real MySQL database for testing.
[CollectionDefinition("RealDatabaseTests", DisableParallelization = true)]
public class RealDatabaseTestCollection
{
}

// The ApiProductIntegrationTests class contains integration tests for the Products API endpoints, which interact with a real MySQL database.
[Collection("RealDatabaseTests")]
public class ApiProductIntegrationTests : IClassFixture<RealMySqlWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiProductIntegrationTests(RealMySqlWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    // Tests that the GetProducts endpoint returns products from the real MySQL database, including their related entities 
    // (Category, ProductStatus, Inventory)
    [Fact]
    public async Task GetProducts_ReturnsProductsFromRealMySqlDatabase()
    {
        // Act
        var response = await _client.GetAsync("/api/Products");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Read the products from the response contentas a list of ProductResponseDto objects, which include the related entities (CategoryName, StatusName, Quantity)
        var products = await response.Content.ReadFromJsonAsync<List<ProductResponseDto>>();

        // Assert that the products list is not null, contains products, and includes the expected product names from the real MySQL database
        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.Contains(products, product => product.Name == "Clean Code");
        Assert.Contains(products, product => product.Name == "The Hobbit");
    }

    // Tests that a product can be created through the API and then retrieved by its ID, 
    // ensuring that the product is correctly saved in the real MySQL database and that the API endpoints for 
    // creating and retrieving products work as expected.
    [Fact]
    public async Task PostProduct_CreatesProductInRealMySqlDatabase_AndCanBeFetchedById()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            CategoryId = 2,
            ProductStatusId = 1,
            Name = "Real API Integration Book",
            Description = "Created through API integration test",
            Price = 199.95m,
            SKU = $"BOOK-REAL-API-{Guid.NewGuid()}",
            Quantity = 4
        };

        // Act
        var postResponse = await _client.PostAsJsonAsync("/api/Products", dto);

        // Assert POST
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        // Read the created product from the POST response to get its ID for the subsequent GET request
        var createdProduct = await postResponse.Content.ReadFromJsonAsync<ProductResponseDto>();

        // Assert the created product details
        Assert.NotNull(createdProduct);
        Assert.True(createdProduct.ProductId > 0);
        Assert.Equal("Real API Integration Book", createdProduct.Name);
        Assert.Equal("Programmering", createdProduct.CategoryName);
        Assert.Equal("Active", createdProduct.StatusName);
        Assert.Equal(4, createdProduct.Quantity);

        // Act GET by ID
        var getResponse = await _client.GetAsync($"/api/Products/{createdProduct.ProductId}");

        // Assert GET by ID
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        // Read the fetched product from the GET response
        var fetchedProduct = await getResponse.Content.ReadFromJsonAsync<ProductResponseDto>();

        // Assert the fetched product details match the created product
        Assert.NotNull(fetchedProduct);
        Assert.Equal(createdProduct.ProductId, fetchedProduct.ProductId);
        Assert.Equal(createdProduct.SKU, fetchedProduct.SKU);
    }
    
    // Tests that attempting to create a product with a SKU that already exists in the real MySQL database returns a Conflict status code, 
    // ensuring that the API correctly enforces unique constraints on the SKU field and returns appropriate error responses when validation fails.
    [Fact]
    public async Task PostProduct_WhenSkuAlreadyExists_ReturnsConflict()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            CategoryId = 2,
            ProductStatusId = 1,
            Name = "Duplicate SKU Book",
            Description = "This should fail",
            Price = 99.95m,
            SKU = "BOOK-CLEAN-CODE",
            Quantity = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Products", dto);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}

// The RealMySqlWebApplicationFactory class is a custom WebApplicationFactory that configures the application to use a real MySQL database for 
// integration testing.
public class RealMySqlWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string StartupConnectionString =
        "Server=127.0.0.1;Port=3306;Database=IdeenOgBogen;User=root;Password=1234;";

    private const string TestConnectionString =
        "Server=127.0.0.1;Port=3306;Database=IdeenOgBogen_IntegrationTest;User=root;Password=1234;";

    public RealMySqlWebApplicationFactory()
    {
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__DefaultConnection",
            StartupConnectionString
        );
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<AppDbContext>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(
                    TestConnectionString,
                    new MySqlServerVersion(new Version(8, 4, 0))
                );
            });

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Ensure the test database is clean before each test run
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        });
    }
}