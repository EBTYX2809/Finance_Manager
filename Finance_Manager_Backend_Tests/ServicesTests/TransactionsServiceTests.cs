﻿using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend_Tests.DataBase;
using Finance_Manager_Backend.BusinessLogic.Services;
using Moq;
using Microsoft.Extensions.Logging;
using Finance_Manager_Backend.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using AutoMapper;
using Finance_Manager_Tests.ServicesTests;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;

namespace Finance_Manager_Backend_Tests.ServicesTests;

[Collection("Database Collection")]
public class TransactionsServiceTests
{
    private readonly AppDbContext _appDbContext;
    private Mock<ILogger<TransactionsService>> _mockLoggerTS;
    private Mock<ILogger<DbTransactionTemplate>> _mockLoggerTT;
    private DbTransactionTemplate _transactionTemplate;
    private TransactionsService _transactionsService;
    private readonly ITestOutputHelper _output;
    private readonly UsersService _usersService;
    private readonly CategoriesService _categoriesService;
    private readonly IMapper _mapper;

    public TransactionsServiceTests(TestDbContextFixture fixture, ITestOutputHelper output)
    {
        _appDbContext = fixture.dbContext;
        _mapper = AutoMapperFotTests.GetMapper();
        _mockLoggerTS = new Mock<ILogger<TransactionsService>>();
        _mockLoggerTT = new Mock<ILogger<DbTransactionTemplate>>();
        _transactionTemplate = new DbTransactionTemplate(_appDbContext, _mockLoggerTT.Object);
        _categoriesService = new CategoriesService(_appDbContext, _mapper);
        _usersService = new UsersService(_appDbContext, null, new MemoryCache(new MemoryCacheOptions())); // converter not need
        _transactionsService = new TransactionsService(_appDbContext, _transactionTemplate, 
            _mockLoggerTS.Object, _usersService, _mapper, _categoriesService);
        _output = output;        
    }

    [Fact]
    public async Task CreateTransactionAsync_Test() 
    {
        // Arrange
        var user = await _appDbContext.Users.FirstOrDefaultAsync();
        var category = await _appDbContext.Categories.FirstOrDefaultAsync();
        var transaction = new Transaction("New transaction form test", 100.89m, DateTime.Now, category, user);
        var transactionDTO = _mapper.Map<TransactionDTO>(transaction);

        // Act
        await _transactionsService.CreateTransactionAsync(transactionDTO);
        var createdTransaction = await _appDbContext.Transactions.FirstOrDefaultAsync(c => c.Id == transactionDTO.Id);
        var createdTransactionDTO = _mapper.Map<TransactionDTO>(createdTransaction);

        // Assert
        createdTransactionDTO.Should().BeEquivalentTo(transactionDTO);
    }

    [Fact]
    public async Task GetTransactionsAsync_Test()
    {
        // Arrange
        var user = await _appDbContext.Users.FirstOrDefaultAsync();
        int transactionsCount = 3;

        // Act
        var recievedTransactions = await _transactionsService.GetTransactionsAsync(user.Id, DateTime.Now, transactionsCount);

        // Assert
        Assert.NotNull(recievedTransactions);
        Assert.Equal(transactionsCount, recievedTransactions.Count);

        foreach (var transaction in recievedTransactions)
        {
            _output.WriteLine(transaction.Name);
        }
    }

    [Fact]
    public async Task UpdateTransactionAsync_Test()
    {
        // Arrange
        var oldTransaction = await _appDbContext.Transactions.FirstOrDefaultAsync();

        // Act
        oldTransaction.Name = "New name";
        var oldTransactionDTO = _mapper.Map<TransactionDTO>(oldTransaction);        

        await _transactionsService.UpdateTransactionAsync(oldTransactionDTO);

        var newTransaction = await _appDbContext.Transactions
            .Include(t => t.Category)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == oldTransaction.Id);

        // Assert
        newTransaction.Should().BeEquivalentTo(oldTransaction);
    }

    [Fact]
    public async Task DeleteTransactionAsync_Test()
    {
        // Arrange
        var transactionForDelete = await _appDbContext.Transactions.FirstOrDefaultAsync();
        int id = transactionForDelete.Id;

        // Act
        await _transactionsService.DeleteTransactionAsync(id);

        var deletedTransaction = await _appDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);

        // Assert
        Assert.Null(deletedTransaction);
    }
}
