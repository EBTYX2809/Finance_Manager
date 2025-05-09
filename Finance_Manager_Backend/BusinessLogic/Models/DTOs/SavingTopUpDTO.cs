using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class SavingTopUpDTO
{
    [JsonProperty(Required = Required.Always)]
    public int savingId { get; set; }

    [JsonProperty(Required = Required.Always)]
    [SwaggerSchema(Format = "decimal")]
    public decimal topUpAmount { get; set; }
}
