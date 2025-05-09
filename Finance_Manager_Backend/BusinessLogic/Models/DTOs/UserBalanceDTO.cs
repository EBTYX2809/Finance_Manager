using Newtonsoft.Json;

namespace Finance_Manager_Backend.BusinessLogic.Models.DTOs;

public class UserBalanceDTO
{
    [JsonProperty(Required = Required.Always)]
    public CurrencyBalanceDTO PrimaryBalance { get; set; }

    [JsonProperty(Required = Required.Default)]
    public CurrencyBalanceDTO? SecondaryBalance1 { get; set; }

    [JsonProperty(Required = Required.Default)]
    public CurrencyBalanceDTO? SecondaryBalance2 { get; set; }
}
