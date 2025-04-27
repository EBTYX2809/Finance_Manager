using AutoMapper;
using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.BusinessLogic.Services;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend_Tests.DataBase;
using Finance_Manager_Tests.ServicesTests;
using FluentAssertions;
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
    private readonly IMapper _mapper;

    public SavingsServiceTests(TestDbContextFixture fixture, ITestOutputHelper output)
    {
        _appDbContext = fixture.dbContext;
        _mapper = AutoMapperFotTests.GetMapper();
        _mockLoggerTS = new Mock<ILogger<SavingsService>>();
        _mockLoggerTT = new Mock<ILogger<DbTransactionTemplate>>();
        _transactionTemplate = new DbTransactionTemplate(_appDbContext, _mockLoggerTT.Object);
        _usersService = new UsersService(_appDbContext, null); // converter not need
        _savingsService = new SavingsService(_appDbContext, _transactionTemplate, _mockLoggerTS.Object, _usersService, _mapper);
        _output = output;
    }

    [Fact]
    public async Task CreateSavingAsync_Test()
    {
        // Arrange
        var user = await _appDbContext.Users.FirstOrDefaultAsync();
        var saving = new Saving("Test Saving", 10000, user);
        var savingDTO = _mapper.Map<SavingDTO>(saving);
        
        // Act
        await _savingsService.CreateSavingAsync(savingDTO);
        var createdSaving = await _appDbContext.Savings.FirstOrDefaultAsync(s => s.Id == savingDTO.Id);
        var createdSavingDTO = _mapper.Map<SavingDTO>(createdSaving);

        // Assert
        createdSavingDTO.Should().BeEquivalentTo(savingDTO);
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
        var starterBalance = oldSaving.User.Balance;
        decimal topUpAmount = 100.50m;

        // Act
        await _savingsService.UpdateSavingAsync(oldSaving.Id, topUpAmount);

        var newSaving = await _appDbContext.Savings.FirstOrDefaultAsync(s => s.Id == oldSaving.Id);

        // Assert
        Assert.Equal(topUpAmount, newSaving.CurrentAmount);
        Assert.Equal(oldSaving.User.Balance, starterBalance - topUpAmount);
    }

    [Fact]
    public async Task UpdateWithExcessSavingAsync_Test()
    {
        // Arrange
        var oldSaving = await _appDbContext.Savings.FirstOrDefaultAsync();
        var starterBalance = oldSaving.User.Balance;
        var requiredTopUp = 50m;
        oldSaving.CurrentAmount = oldSaving.Goal - requiredTopUp;
        await _appDbContext.SaveChangesAsync();
        decimal topUpAmount = 100.50m;

        // Act
        await _savingsService.UpdateSavingAsync(oldSaving.Id, topUpAmount);

        var newSaving = await _appDbContext.Savings.FirstOrDefaultAsync(s => s.Id == oldSaving.Id);

        // Assert
        Assert.Equal(oldSaving.Goal, newSaving.CurrentAmount);
        Assert.Equal(newSaving.User.Balance, starterBalance - requiredTopUp);
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
