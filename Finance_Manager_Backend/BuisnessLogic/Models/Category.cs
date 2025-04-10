namespace Finance_Manager_Backend.BuisnessLogic.Models;

public class Category
{
    public Category() { }
    public Category(string name, bool isIncome, string icon, string colorForBackground)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name can't be null or empty", nameof(name));

        if (string.IsNullOrWhiteSpace(icon))
            throw new ArgumentException("Icon can't be null or empty", nameof(icon));

        if (string.IsNullOrWhiteSpace(colorForBackground))
            throw new ArgumentException("ColorForBackground can't be null or empty", nameof(colorForBackground));

        Name = name;
        IsIncome = isIncome;
        Icon = icon;
        ColorForBackground = colorForBackground;
    }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsIncome { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string ColorForBackground { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
    public List<Category>? InnerCategories { get; set; } 
    public Category? ParentCategory { get; set; }
}
