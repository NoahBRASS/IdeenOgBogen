using System.ComponentModel.DataAnnotations;

namespace IdeenOgBogen.Application.DTOs.Products;

public class CreateProductDto
{
    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int ProductStatusId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(0, 999999)]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}