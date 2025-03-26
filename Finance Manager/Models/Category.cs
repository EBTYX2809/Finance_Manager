namespace Finance_Manager.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string ColorForBackground { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
    public List<Category>? InnerCategories { get; set; } 

    // Навигационные свойства для связи
    public Category? ParentCategory { get; set; }
}
