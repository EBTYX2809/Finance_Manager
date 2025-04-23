namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class AnalyticsQueryDTO
{    
    public int userId { get; set; }
    public DateTime minDate { get; set; }
    public DateTime maxDate { get; set; }
}
