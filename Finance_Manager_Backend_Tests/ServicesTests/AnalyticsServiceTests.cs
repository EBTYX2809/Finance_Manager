using Finance_Manager_Backend.BusinessLogic.Services;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend_Tests.DataBase;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Finance_Manager_Backend_Tests.ServicesTests;

[Collection("Database Collection")]
public class AnalyticsServiceTests
{
    private readonly AppDbContext _appDbContext;
    private AnalyticsService _analyticsService;
    private readonly ITestOutputHelper _output;
    private readonly UsersService _usersService;
    private readonly CategoriesService _categoriesService;

    public AnalyticsServiceTests(TestDbContextFixture fixture, ITestOutputHelper output)
    {
        _appDbContext = fixture.dbContext;       
        _output = output;
        _usersService = new UsersService(_appDbContext);
        _categoriesService = new CategoriesService(_appDbContext);
        _analyticsService = new AnalyticsService(_appDbContext, _usersService , _categoriesService);
    }

    [Fact]
    public async Task GetAlalyticsFromDate_Test()
    {
        // Arrange
        var user = await _appDbContext.Users.FirstOrDefaultAsync();

        foreach (var transaction in _appDbContext.Transactions)
        {
            transaction.User = user;
        }

        // Act
        var result = await _analyticsService.GetAlalyticFromDateAsync(user.Id, new DateTime(2025, 1, 1), new DateTime(2025, 6, 1));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.Values.Sum()); // 100%

        foreach(var r in result)
        {
            _output.WriteLine($"{r.Key.Name}: {r.Value}");
        }
    }
}
