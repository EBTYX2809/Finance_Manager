namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class UserBalanceDTO
{
    public CurrencyBalanceDTO PrimaryBalance { get; set; }
    public CurrencyBalanceDTO? SecondaryBalance1 { get; set; }
    public CurrencyBalanceDTO? SecondaryBalance2 { get; set; }
/*    public (string, decimal) PrimaryBalance { get; set; }
    public (string, decimal?)? SecondaryBalance1 { get; set; }
    public (string, decimal?)? SecondaryBalance2 { get; set; }*/
}
