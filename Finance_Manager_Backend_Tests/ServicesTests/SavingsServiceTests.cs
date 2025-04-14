using Finance_Manager_Backend.BuisnessLogic.Models;
using Finance_Manager_Backend.BuisnessLogic.Services;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend_Tests.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace Finance_Manager_Backend_Tests.ServicesTests;

[Collection("Database Collection")]
public class SavingsServiceTests
{
    private readonly AppDbContext _appDbContext;
    private Mock<ILogger<SavingsService>> _mockLoggerTS;
    private Mock<ILogger<DbTransactionTemplate>> _mockLoggerTT;
    private DbTransactionTemplate _transactionTemplate;
    private SavingsService _savingsService;
    private readonly ITestOutputHelper _output;
    private readonly UsersService _usersService;

    public SavingsServiceTests(TestDbContextFixture fixture, ITestOutputHelper output)
    {
        _appDbContext = fixture.dbContext;
        _mockLoggerTS = new Mock<ILogger<SavingsService>>();
        _mockLoggerTT = new Mock<ILogger<DbTransactionTemplate>>();
        _transactionTemplate = new DbTransactionTemplate(_appDbContext, _mockLoggerTT.Object);
        _usersService = new UsersService(_appDbContext);
        _savingsService = new SavingsService(_appDbContext, _transactionTemplate, _mockLoggerTS.Object, _usersService);
        _output = output;
    }

    [Fact]
    public async Task CreateSavingAsync_Test()
    {
        // Arrange
        var user = await _appDbContext.Users.FirstOrDefaultAsync();
        var saving = new Saving("Test Saving", 10000, user);
        
        // Act
        await _savingsService.CreateSavingAsync(saving);
        var createdSaving = await _appDbContext.Savings.FirstOrDefaultAsync(s => s.Id == saving.Id);

        // Assert
        Assert.Equal(saving, createdSaving);
    }

    [Fact]
    public async Task GetSavingsAsync_Test()
    {
        // Arrange
        var user = await _appDbContext.Users.FirstOrDefaultAsync();
        int transactionsCount = 2;

        // Act
        var recievedSavings = await _savingsService.GetSavingsAsync(user.Id, 0, transactionsCount);

        // Assert
        Assert.NotNull(recievedSavings);
        Assert.Equal(transactionsCount, recievedSavings.Count);

        foreach (var saving in recievedSavings)
        {
            _output.WriteLine(saving.Name);
        }
    }

    [Fact]
    public async Task UpdateSavingAsync_Test()
    {
        // Arrange
        var oldSaving = await _appDbContext.Savings.FirstOrDefaultAsync();
        decimal topUpAmount = 100.50m;

        // Act
        await _savingsService.UpdateSavingAsync(oldSaving.Id, topUpAmount);

        var newSaving = await _appDbContext.Savings.FirstOrDefaultAsync(s => s.Id == oldSaving.Id);

        // Assert
        Assert.Equal(topUpAmount, newSaving.CurrentAmount);
    }

    [Fact]
    public async Task DeleteSavingAsync_Test()
    {
        // Arrange
        var savingForDelete = await _appDbContext.Savings.FirstOrDefaultAsync();
        int id = savingForDelete.Id;

        // Act
        await _savingsService.DeleteSavingAsync(id);

        var deletedSaving = await _appDbContext.Savings.FirstOrDefaultAsync(t => t.Id == id);

        // Assert
        Assert.Null(deletedSaving);
    }
}
