using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Finance_Manager_Backend.BusinessLogic.Services;

public class UsersService
{
    private AppDbContext _appDbContext;
    private readonly CurrencyConverterService _converter;
    private readonly IMemoryCache _cache;
    public UsersService(AppDbContext appDbContext, CurrencyConverterService converter, IMemoryCache cache)
    {
        _appDbContext = appDbContext;
        _converter = converter;
        _cache = cache;
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) throw new EntityNotFoundException<User>(userId);

        return user;
    }

    public async Task<UserBalanceDTO> GetUserBalanceByIdAsync(int userId)
    {
        var user = await GetUserByIdAsync(userId);

        var cur1 = await _converter.ConvertAsync(user.Balance, user.PrimaryCurrency, user.SecondaryCurrency1);
        var cur2 = await _converter.ConvertAsync(user.Balance, user.PrimaryCurrency, user.SecondaryCurrency2);

        return new UserBalanceDTO()
        {
            PrimaryBalance = new CurrencyBalanceDTO { Currency = user.PrimaryCurrency, Balance = user.Balance },
            SecondaryBalance1 = cur1 == null ? null : new CurrencyBalanceDTO { Currency = user.SecondaryCurrency1, Balance = (decimal)cur1 },
            SecondaryBalance2 = cur2 == null ? null : new CurrencyBalanceDTO { Currency = user.SecondaryCurrency2, Balance = (decimal)cur2 }
        };
    }

    public async Task UpdateUserCurrencyAsync(int id, string currencyRang, string currencyCode)
    {
        var user = await GetUserByIdAsync(id);

        if (currencyRang == "Primary")
        {
            var newBalance = await _converter.ConvertAsync(user.Balance, user.PrimaryCurrency, currencyCode);
            user.Balance = (decimal)newBalance;
            user.PrimaryCurrency = currencyCode;
        }
        else if (currencyRang == "Secondary1") user.SecondaryCurrency1 = currencyCode;
        else if (currencyRang == "Secondary2") user.SecondaryCurrency2 = currencyCode;
        else throw new InvalidOperationException("Invalid currencyRang.");

        await _appDbContext.SaveChangesAsync();
    }
}
