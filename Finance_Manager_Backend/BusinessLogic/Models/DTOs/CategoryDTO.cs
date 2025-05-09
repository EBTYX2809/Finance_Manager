using Newtonsoft.Json;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class CategoryDTO
{
    [JsonProperty(Required = Required.Always)]
    public int Id { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string Name { get; set; } = string.Empty;

    [JsonProperty(Required = Required.Always)]
    public bool IsIncome { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string Icon { get; set; } = string.Empty;

    [JsonProperty(Required = Required.Always)]
    public string ColorForBackground { get; set; } = string.Empty;

    [JsonProperty(Required = Required.Default)]
    public int? ParentCategoryId { get; set; }

    [JsonProperty(Required = Required.Default)]
    public List<CategoryDTO>? InnerCategories { get; set; }
}
