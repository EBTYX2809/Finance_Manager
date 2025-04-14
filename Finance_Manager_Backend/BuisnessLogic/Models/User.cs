namespace Finance_Manager_Backend.BuisnessLogic.Models;

public class User : IEntity
{
    public User() { }
    public User(string email, string salt, string passwordHash, decimal balance)
    {        
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email can't be null or empty", nameof(email));
        
        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentException("Salt can't be null or empty", nameof(salt));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("PasswordHash can't be null or empty", nameof(passwordHash));        

        Email = email;
        Salt = salt;
        PasswordHash = passwordHash;
        Balance = balance;
    }
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public List<Transaction> Transactions { get; set; } = new();
    public List<Saving> Savings { get; set; } = new();    
}