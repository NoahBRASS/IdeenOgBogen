namespace IdeenOgBogen.Domain.Models;

public class Inventory
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public Product? Product { get; set; }
}