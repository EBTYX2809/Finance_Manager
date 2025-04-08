namespace Finance_Manager_Backend.BuisnessLogic.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public Category Category { get; set; } = new();
    public int CategoryId { get; set; }
    public Category? InnerCategory { get; set; }
    public int? InnerCategoryId { get; set; }
    public string? Photo { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = new();
}
