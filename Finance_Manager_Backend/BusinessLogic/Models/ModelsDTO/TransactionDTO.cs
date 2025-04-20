namespace Finance_Manager_Backend.BusinessLogic.Models.ModelsDTO;

public class TransactionDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;   
    public int CategoryId { get; set; }   
    public int? InnerCategoryId { get; set; }
    //public string? Photo { get; set; }
    public int UserId { get; set; }   
}
