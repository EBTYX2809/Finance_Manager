using Newtonsoft.Json;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class UpdateUserCurrencyQueryDTO
{
    [JsonProperty(Required = Required.Always)]
    public int UserId { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string CurrencyRang { get; set; } // Primary, secondary1 or secondary2

    [JsonProperty(Required = Required.Always)]
    public string CurrencyCode { get; set; } // Name
}
