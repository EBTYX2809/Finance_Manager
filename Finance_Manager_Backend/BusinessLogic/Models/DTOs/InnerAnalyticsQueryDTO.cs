namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class InnerAnalyticsQueryDTO
{
    public int parentCategoryId { get; set; }
    public int userId { get; set; }
    public DateTime minDate { get; set; }
    public DateTime maxDate { get; set; }
}
