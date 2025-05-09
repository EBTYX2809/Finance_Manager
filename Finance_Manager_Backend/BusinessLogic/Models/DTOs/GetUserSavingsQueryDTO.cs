using Newtonsoft.Json;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class GetUserSavingsQueryDTO
{
    [JsonProperty(Required = Required.Always)]
    public int userId { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int previousSavingId { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int pageSize { get; set; } = 5;
}
