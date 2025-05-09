using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class TransactionDTO
{
    [JsonProperty(Required = Required.Always)]
    public int Id { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string Name { get; set; } = string.Empty;

    [JsonProperty(Required = Required.Always)]
    [SwaggerSchema(Format = "decimal")]
    public decimal Price { get; set; }

    [JsonProperty(Required = Required.Always)]
    public DateTime Date { get; set; } = DateTime.Now;

    [JsonProperty(Required = Required.Always)]
    public int CategoryId { get; set; }

    [JsonProperty(Required = Required.Default)]
    public int? InnerCategoryId { get; set; }
    //public string? Photo { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int UserId { get; set; }
}
