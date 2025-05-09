using Newtonsoft.Json;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class InnerAnalyticsQueryDTO
{
    [JsonProperty(Required = Required.Always)]
    public int parentCategoryId { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int userId { get; set; }

    [JsonProperty(Required = Required.Always)]
    public DateTime minDate { get; set; }

    [JsonProperty(Required = Required.Always)]
    public DateTime maxDate { get; set; }
}
