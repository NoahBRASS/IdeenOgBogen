namespace IdeenOgBogen.Application.DTOs.Products;

public class ProductResponseDto
{
    public int ProductId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public decimal Price { get; set; }
    public string SKU { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;
    public string StatusName { get; set; } = string.Empty;

    public int Quantity { get; set; }
}