using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class CurrencyBalanceDTO
{
    [JsonProperty(Required = Required.Always)]
    public string Currency { get; set; }

    [JsonProperty(Required = Required.Always)]
    [SwaggerSchema(Format = "decimal")]
    public decimal Balance { get; set; }
}
