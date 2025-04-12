using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend_Tests.DataBase;
using Finance_Manager_Backend.BuisnessLogic.Services;
using Moq;
using Microsoft.Extensions.Logging;
using Finance_Manager_Backend.BuisnessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend_Tests.ServicesTests;

public class TransactionsServiceTests : IClassFixture<TestDbContextFixture>
{
    private readonly AppDbContext _appDbContext;
    private Mock<ILogger<TransactionsService>> _mockLoggerTS;
    private Mock<ILogger<DbTransactionTemplate>> _mockLoggerTT;
    private DbTransactionTemplate _transactionTemplate;
    private TransactionsService _transactionsService;

    public TransactionsServiceTests(TestDbContextFixture fixture)
    {
        _appDbContext = fixture.dbContext;
        _mockLoggerTS = new Mock<ILogger<TransactionsService>>();
        _mockLoggerTT = new Mock<ILogger<DbTransactionTemplate>>();
        _transactionTemplate = new DbTransactionTemplate(_appDbContext, _mockLoggerTT.Object);
        _transactionsService = new TransactionsService(_appDbContext, _transactionTemplate, _mockLoggerTS.Object);
    }

    [Fact]
    public async Task CreateTransactionAsync_Test() 
    {
        // Arrange
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == 1);
        var category = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Id == 1);
        var transaction = new Transaction("New transaction form test", 100.89m, DateTime.Now, category, user);

        // Act
        await _transactionsService.CreateTransactionAsync(transaction);
        var createdTransaction = await _appDbContext.Transactions
            .Include(t => t.Category)   
            .Include(t => t.User)
            .FirstOrDefaultAsync(c => c.Id == transaction.Id);

        // Assert
        Assert.Equal(transaction, createdTransaction);
    }

    [Fact]
    public async Task GetTransactionsAsync_Test()
    {
        // Arrange
        int transactionsCount = 3;

        // Act
        var recievedTransactions = await _transactionsService.GetTransactionsAsync(1, DateTime.Now, transactionsCount);

        // Assert
        Assert.NotNull(recievedTransactions);
        Assert.Equal(transactionsCount, recievedTransactions.Count);
    }

    [Fact]
    public async Task UpdateTransactionAsync_Test()
    {
        // Arrange
        var oldTransaction = await _appDbContext.Transactions.FirstOrDefaultAsync(t => t.UserId == 1);

        // Act
        oldTransaction.Name = "New name";

        await _transactionsService.UpdateTransactionAsync(oldTransaction);

        var newTransaction = await _appDbContext.Transactions
            .Include(t => t.Category)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == oldTransaction.Id);

        // Assert
        Assert.Equal(oldTransaction, newTransaction);
    }

    [Fact]
    public async Task DeleteTransactionAsync_Test()
    {
        // Arrange
        var transactionForDelete = await _appDbContext.Transactions.FirstOrDefaultAsync(t => t.UserId == 1);
        int id = transactionForDelete.Id;

        // Act
        await _transactionsService.DeleteTransactionAsync(id);

        var deletedTransaction = await _appDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);

        // Assert
        Assert.Null(deletedTransaction);
    }
}
