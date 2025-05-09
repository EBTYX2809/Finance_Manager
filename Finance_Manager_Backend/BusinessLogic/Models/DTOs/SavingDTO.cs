using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class SavingDTO
{
    [JsonProperty(Required = Required.Always)]
    public int Id { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string Name { get; set; } = string.Empty;

    [JsonProperty(Required = Required.Always)]
    [SwaggerSchema(Format = "decimal")]
    public decimal Goal { get; set; }

    [JsonProperty(Required = Required.Always)]
    [SwaggerSchema(Format = "decimal")]
    public decimal CurrentAmount { get; set; } = 0;

    [JsonProperty(Required = Required.Always)]
    public int UserId { get; set; }
}
