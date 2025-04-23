namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class AuthDataDTO
{
    private string _email = string.Empty;
    public string email
    {
        get => _email;
        set => _email = value?.Trim() ?? string.Empty; // For delete spaces from email
    }
    public string password { get; set; } = string.Empty;
}
