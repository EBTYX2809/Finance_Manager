namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class GetUserTransactionsQueryDTO
{
    public int userId { get; set; }
    public DateTime? lastDate { get; set; }
    public int pageSize { get; set; } = 20;
}
