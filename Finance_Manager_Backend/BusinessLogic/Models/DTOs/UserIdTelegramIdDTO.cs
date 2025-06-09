using Newtonsoft.Json;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class UserIdTelegramIdDTO
{
    [JsonProperty(Required = Required.Always)]
    public int UserId { get; set; }

    [JsonProperty(Required = Required.Always)]
    public long TelegramId { get; set; }
}