namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class UpdateUserCurrencyQueryDTO
{
    public int UserId { get; set; }
    public string CurrencyRang { get; set; } // Primary, secondary1 or secondary2
    public string CurrencyCode { get; set; } // Name
}
