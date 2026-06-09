using IdeenOgBogen.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace IdeenOgBogen.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ProductStatus> ProductStatuses => Set<ProductStatus>();
    public DbSet<Inventory> Inventories => Set<Inventory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.CategoryId);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Description)
                .HasMaxLength(255);

            entity.HasOne(c => c.ParentCategory)
                .WithMany(c => c.ChildCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductStatus>(entity =>
        {
            entity.HasKey(s => s.ProductStatusId);

            entity.Property(s => s.StatusName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.ProductId);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Description)
                .HasMaxLength(500);

            entity.Property(p => p.Price)
                .HasColumnType("decimal(10,2)");

            entity.Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(p => p.SKU)
                .IsUnique();

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            entity.HasOne(p => p.ProductStatus)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.ProductStatusId);

            entity.HasOne(p => p.Inventory)
                .WithOne(i => i.Product)
                .HasForeignKey<Inventory>(i => i.ProductId);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(i => i.ProductId);

            entity.Property(i => i.Quantity)
                .IsRequired();
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                CategoryId = 1,
                ParentCategoryId = null,
                Name = "Bøger",
                Description = "Alle bøger"
            },
            new Category
            {
                CategoryId = 2,
                ParentCategoryId = 1,
                Name = "Programmering",
                Description = "Bøger om kode og software"
            },
            new Category
            {
                CategoryId = 3,
                ParentCategoryId = 1,
                Name = "Fantasy",
                Description = "Fantasy bøger"
            }
        );

        modelBuilder.Entity<ProductStatus>().HasData(
            new ProductStatus
            {
                ProductStatusId = 1,
                StatusName = "Active",
                IsSellable = true
            },
            new ProductStatus
            {
                ProductStatusId = 2,
                StatusName = "OutOfStock",
                IsSellable = false
            },
            new ProductStatus
            {
                ProductStatusId = 3,
                StatusName = "Discontinued",
                IsSellable = false
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1,
                CategoryId = 2,
                ProductStatusId = 1,
                Name = "Clean Code",
                Description = "Bog om god softwareudvikling",
                Price = 299.95m,
                SKU = "BOOK-CLEAN-CODE",
                CreatedAt = new DateTime(2026, 1, 1)
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
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );

        modelBuilder.Entity<Inventory>().HasData(
            new Inventory
            {
                ProductId = 1,
                Quantity = 10,
                LastUpdated = new DateTime(2026, 1, 1)
            },
            new Inventory
            {
                ProductId = 2,
                Quantity = 5,
                LastUpdated = new DateTime(2026, 1, 1)
            }
        );
    }
}