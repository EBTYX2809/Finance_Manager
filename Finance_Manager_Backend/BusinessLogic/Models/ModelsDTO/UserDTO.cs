namespace Finance_Manager_Backend.BusinessLogic.Models.ModelsDTO;

public class UserDTO
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}
