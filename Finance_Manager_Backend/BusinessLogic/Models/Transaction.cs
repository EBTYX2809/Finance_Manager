namespace Finance_Manager_Backend.BusinessLogic.Models;

public class Transaction : IEntity
{
    public Transaction() { }
    public Transaction(string name, decimal price, DateTime date, Category category, User user)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name can't be null or empty", nameof(name));

        if(category == null) 
            throw new ArgumentNullException("Category can't be null", nameof(category));

        if(user == null) 
            throw new ArgumentNullException("User can't be null", nameof(user));

        Name = name;
        Price = price;
        Date = date;
        Category = category;
        CategoryId = category.Id;
        User = user;
        UserId = user.Id;
    }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
    public Category? InnerCategory { get; set; }
    public int? InnerCategoryId { get; set; }
    public string? Photo { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}
