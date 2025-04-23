namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class AuthUserTokenDTO
{
    public UserDTO UserDTO { get; set; } = new();
    public string Token { get; set; } = string.Empty;
}
