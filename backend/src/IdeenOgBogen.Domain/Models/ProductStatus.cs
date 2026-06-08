namespace IdeenOgBogen.Domain.Models;

public class ProductStatus
{
    public int ProductStatusId { get; set; }

    public string StatusName { get; set; } = string.Empty;
    public bool IsSellable { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}