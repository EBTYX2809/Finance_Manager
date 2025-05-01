namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class AuthUserTokensDTO
{
    public UserDTO UserDTO { get; set; } = new();
    public string AccessJwtToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
