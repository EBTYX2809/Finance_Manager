namespace Finance_Manager_Backend.BusinessLogic.Models.ModelsDTO;

public class SavingDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Goal { get; set; }
    public decimal CurrentAmount { get; set; } = 0;
    public int UserId { get; set; }    
}
