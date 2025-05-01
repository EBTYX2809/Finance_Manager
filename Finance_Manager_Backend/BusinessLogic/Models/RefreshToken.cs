namespace Finance_Manager_Backend.BusinessLogic.Models;

public class RefreshToken : IEntity
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRevoked { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
