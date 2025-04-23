namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class CategoryPercentDTO
{
    public CategoryDTO CategoryDTO { get; set; } = new();
    public float Percent { get; set; }
}
