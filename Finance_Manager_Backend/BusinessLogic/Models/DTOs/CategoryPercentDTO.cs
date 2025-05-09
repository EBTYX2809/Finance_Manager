using Newtonsoft.Json;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class CategoryPercentDTO
{
    [JsonProperty(Required = Required.Always)]
    public CategoryDTO CategoryDTO { get; set; } = new();

    [JsonProperty(Required = Required.Always)]
    public float Percent { get; set; }
}
