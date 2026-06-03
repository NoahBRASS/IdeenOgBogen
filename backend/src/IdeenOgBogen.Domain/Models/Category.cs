namespace IdeenOgBogen.Domain.Models;

public class Category
{
    public int CategoryId { get; set; }
    public int? ParentCategoryId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Category? ParentCategory { get; set; }
    public ICollection<Category> ChildCategories { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}