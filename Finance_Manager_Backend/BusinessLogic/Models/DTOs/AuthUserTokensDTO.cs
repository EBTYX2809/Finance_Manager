using Newtonsoft.Json;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class AuthUserTokensDTO
{
    [JsonProperty(Required = Required.Always)]
    public UserDTO UserDTO { get; set; } = new();

    [JsonProperty(Required = Required.Always)]
    public string AccessJwtToken { get; set; } = string.Empty;

    [JsonProperty(Required = Required.Always)]
    public string RefreshToken { get; set; } = string.Empty;
}
