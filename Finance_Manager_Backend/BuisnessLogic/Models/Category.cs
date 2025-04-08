namespace Finance_Manager_Backend.BuisnessLogic.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsIncome { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string ColorForBackground { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
    public List<Category>? InnerCategories { get; set; } 
    public Category? ParentCategory { get; set; }
}
