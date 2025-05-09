using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class UserDTO
{
    [JsonProperty(Required = Required.Always)]
    public int Id { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string Email { get; set; } = string.Empty;

    [JsonProperty(Required = Required.Always)]
    [SwaggerSchema(Format = "decimal")]
    public decimal Balance { get; set; }
}
