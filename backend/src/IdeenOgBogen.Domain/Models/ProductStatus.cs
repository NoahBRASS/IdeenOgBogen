namespace IdeenOgBogen.Domain.Models;

public class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }
    public int ProductStatusId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public decimal Price { get; set; }
    public string SKU { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Category? Category { get; set; }
    public ProductStatus? ProductStatus { get; set; }
    public Inventory? Inventory { get; set; }
}