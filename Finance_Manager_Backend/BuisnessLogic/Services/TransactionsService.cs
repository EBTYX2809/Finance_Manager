using Microsoft.EntityFrameworkCore;
using System.Data;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using UserTransaction = Finance_Manager_Backend.BuisnessLogic.Models.Transaction;

namespace Finance_Manager_Backend.BuisnessLogic.Services;

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
    // Need return id in controller to front
    public async Task CreateTransactionAsync(UserTransaction userTransaction)
    {
        await _dbTransactionTemplate.ExecuteTransactionAsync(async () =>
        {
            _logger.LogInformation("Executing CreateTransactionAsync method.");
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userTransaction.UserId);

            if (user == null) throw new UserNotFoundException(userTransaction.UserId.ToString());

            await _appDbContext.Transactions.AddAsync(userTransaction);

            if (userTransaction.Category.IsIncome) user.Balance += userTransaction.Price;
            else user.Balance -= userTransaction.Price;            
        });
    }

    public async Task<List<UserTransaction>> GetTransactionsAsync(int userId, DateTime? lastDate, int pageSize)
    {
        var orderedTransactions = _appDbContext.Transactions
            .Include(t => t.Category)
            .Include(t => t.InnerCategory)
            .Include(t => t.User)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date);

        if (lastDate.HasValue)
        {
            orderedTransactions = orderedTransactions
                .Where(t => t.Date < lastDate.Value) 
                as IOrderedQueryable<UserTransaction>;
        }

        return await orderedTransactions
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task UpdateTransactionAsync(UserTransaction newUserTransaction)
    {
        _logger.LogInformation("Executing UpdateTransactionAsync method.");
        await _dbTransactionTemplate.ExecuteTransactionAsync(async () =>
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == newUserTransaction.UserId);

            if (user == null) throw new UserNotFoundException(newUserTransaction.UserId.ToString());

            var oldUserTransaction = await _appDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == newUserTransaction.Id);

            if (oldUserTransaction == null) throw new TransactionIsNotExistException(newUserTransaction.Id.ToString());

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
        _logger.LogInformation("Executing DeleteTransactionAsync method.");
        await _dbTransactionTemplate.ExecuteTransactionAsync(async () =>
        {            
            var transaction = await _appDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);

            if (transaction == null) throw new TransactionIsNotExistException(transactionId.ToString());

            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == transaction.UserId);

            if (user == null) throw new UserNotFoundException(transaction.UserId.ToString());

            user.Balance += transaction.Price;

            _appDbContext.Transactions.Remove(transaction);
        });
    }    
}
