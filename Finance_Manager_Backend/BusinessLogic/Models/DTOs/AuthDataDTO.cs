using Newtonsoft.Json;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class AuthDataDTO
{
    private string _email = string.Empty;

    [JsonProperty(Required = Required.Always)]
    public string email
    {
        get => _email;
        set => _email = value?.Trim() ?? string.Empty; // For delete spaces from email
    }

    [JsonProperty(Required = Required.Always)]
    public string password { get; set; } = string.Empty;
}
