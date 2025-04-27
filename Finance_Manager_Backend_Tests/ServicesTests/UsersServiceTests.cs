using Finance_Manager_Backend.BusinessLogic.Services;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend_Tests.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Finance_Manager_Tests.ServicesTests;

[Collection("Database Collection")]
public class UsersServiceTests
{
    private AppDbContext _appDbContext;
    private readonly UsersService _usersService;    
    public UsersServiceTests(TestDbContextFixture fixture)
    {
        _appDbContext = fixture.dbContext;

        HttpClient client = new HttpClient();

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build();

        CurrencyConverterService converter = new CurrencyConverterService(client, config, new MemoryCache(new MemoryCacheOptions()));
        _usersService = new UsersService(_appDbContext, converter);
    }

    [Fact]
    public async Task GetUserBalanceByIdAsync_Test()
    {
        // Arrange
        var user = await _appDbContext.Users.FirstOrDefaultAsync();
        //user.PrimaryCurrency = "USD"; // Theoreticaly USD will be in default
        await _usersService.UpdateUserCurrency(user.Id, "Secondary1", "EUR");
        await _usersService.UpdateUserCurrency(user.Id, "Secondary2", "UAH");
        //user.SecondaryCurrency1 = "EUR";
        //user.SecondaryCurrency2 = "UAH";

        // Act
        var result = await _usersService.GetUserBalanceByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Console.WriteLine($"First currency({result.PrimaryBalance.Item1}) balance: {result.PrimaryBalance.Item2}.");
        Console.WriteLine($"Second currency({result.SecondaryBalance1.Value.Item1}) balance: {result.SecondaryBalance1.Value.Item2}.");
        Console.WriteLine($"Third currency({result.SecondaryBalance2.Value.Item1}) balance: {result.SecondaryBalance2.Value.Item2}.");
    }
}
