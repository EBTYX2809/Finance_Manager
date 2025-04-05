namespace Finance_Manager_Backend.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public List<Transaction> Transactions { get; set; } = new();
    public List<Saving> Savings { get; set; } = new();    
}