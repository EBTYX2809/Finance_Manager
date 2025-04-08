using Finance_Manager_Backend.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UserTransaction = Finance_Manager_Backend.Models.Transaction;

namespace Finance_Manager_Backend.Services;

public class TransactionsService
{
    private AppDbContext _appDbContext;
    private DbTransactionTemplate _dbTransactionTemplate;
    private ILogger<TransactionsService> _logger;
    public TransactionsService(AppDbContext appDbContext, DbTransactionTemplate dbTransactionTemplate, ILogger<TransactionsService> logger)
    {
        _appDbContext = appDbContext;
        _dbTransactionTemplate = dbTransactionTemplate;
        _logger = logger;
    }

    // Db Transactions required due to changing user balance
    // For test
    public async Task CreateTransactionAsync(UserTransaction userTransaction)
    {
        await _dbTransactionTemplate.ExecuteTransactionAsync(async () =>
        {
            _logger.LogInformation("Executing CreateTransactionAsync method.");
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userTransaction.UserId);

            if (user == null)
            {
                _logger.LogError("User with ID {UserId} not found.", userTransaction.UserId);
                throw new InvalidOperationException("User not found");
            }

            await _appDbContext.Transactions.AddAsync(userTransaction);

            if (userTransaction.Category.IsIncome) user.Balance += userTransaction.Price;
            else user.Balance -= userTransaction.Price;
        });
    }

    // For test
    public async Task<List<UserTransaction>> GetTransactionsAsync(int userId, DateTime? lastDate, int pageSize)
    {
        var orderedTransactions = _appDbContext.Transactions
            .Include(t => t.Category)
            .Include(t => t.InnerCategory)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date);

        if (lastDate.HasValue)
        {
            orderedTransactions = orderedTransactions.Where(t => t.Date < lastDate.Value) as IOrderedQueryable<UserTransaction>;
        }

        return await orderedTransactions
            .Take(pageSize)
            .ToListAsync();
    }

    // For test
    public async Task UpdateTransactionAsync(UserTransaction newUserTransaction)
    {
        _logger.LogInformation("Executing UpdateTransactionAsync method.");
        await _dbTransactionTemplate.ExecuteTransactionAsync(async () =>
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == newUserTransaction.UserId);

            if (user == null)
            {
                _logger.LogError("User with ID {UserId} not found.", newUserTransaction.UserId);
                throw new InvalidOperationException("User not found");
            }

            var oldUserTransaction = await _appDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == newUserTransaction.UserId);

            if (oldUserTransaction == null)
            {
                _logger.LogError("Transaction isn't exist.");
                throw new InvalidOperationException("Transaction isn't exist.");
            }

            if (newUserTransaction.Price >= oldUserTransaction.Price)
            {
                user.Balance -= newUserTransaction.Price - oldUserTransaction.Price;
            }
            else if (newUserTransaction.Price <= oldUserTransaction.Price)
            {
                user.Balance += oldUserTransaction.Price - newUserTransaction.Price;
            }

            oldUserTransaction.Name = newUserTransaction.Name;
            oldUserTransaction.Price = newUserTransaction.Price;
            oldUserTransaction.Date = newUserTransaction.Date;

            oldUserTransaction.CategoryId = newUserTransaction.CategoryId;
            oldUserTransaction.Category = newUserTransaction.Category;

            oldUserTransaction.InnerCategoryId = newUserTransaction.InnerCategoryId;
            oldUserTransaction.InnerCategory = newUserTransaction.InnerCategory;

            oldUserTransaction.Photo = newUserTransaction.Photo;
        });
    }

    public async Task DeleteTransactionAsync(int transactionId)
    {
        await _dbTransactionTemplate.ExecuteTransactionAsync(async () =>
        {
            _logger.LogInformation("Executing DeleteTransactionAsync method.");
            var transaction = await _appDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);

            if (transaction == null)
            {
                _logger.LogError("Transaction isn't exist.");
                throw new InvalidOperationException("Transaction isn't exist.");
            }

            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == transaction.UserId);

            if (user == null)
            {
                _logger.LogError("User with ID {UserId} not found.", transaction.UserId);
                throw new InvalidOperationException("User not found");
            }

            user.Balance += transaction.Price;

            _appDbContext.Transactions.Remove(transaction);
        });
    }    
}
