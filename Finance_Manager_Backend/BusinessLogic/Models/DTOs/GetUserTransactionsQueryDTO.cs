using Newtonsoft.Json;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class GetUserTransactionsQueryDTO
{
    [JsonProperty(Required = Required.Always)]
    public int userId { get; set; }

    [JsonProperty(Required = Required.Default)]
    public DateTime? lastDate { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int pageSize { get; set; } = 20;
}
