namespace Finance_Manager_Backend.Models;

public class Saving
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Goal { get; set; }
    public decimal? CurrentAmount { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = new();   
}
