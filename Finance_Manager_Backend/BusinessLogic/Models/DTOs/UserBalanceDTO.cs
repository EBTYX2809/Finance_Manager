namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class UserBalanceDTO
{
    public CurrencyBalanceDTO PrimaryBalance { get; set; }
    public CurrencyBalanceDTO? SecondaryBalance1 { get; set; }
    public CurrencyBalanceDTO? SecondaryBalance2 { get; set; }
}
