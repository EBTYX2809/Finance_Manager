namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class GetUserSavingsQueryDTO
{
    public int userId { get; set;  }
    public int previousSavingId { get; set; }
    public int pageSize { get; set; } = 5;
}
