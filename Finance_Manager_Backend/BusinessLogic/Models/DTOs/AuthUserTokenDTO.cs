namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class AuthUserTokenDTO
{
    public UserDTO UserDTO { get; set; } = new();
    public string AccessJwtToken { get; set; } = string.Empty;
    public RefreshToken RefreshToken { get; set; } = new();
}
