using Microsoft.EntityFrameworkCore;
using System.Data;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using UserTransaction = Finance_Manager_Backend.BusinessLogic.Models.Transaction;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using AutoMapper;

namespace Finance_Manager_Backend.BusinessLogic.Services;

public class TransactionsService
{
    private AppDbContext _appDbContext;
    private DbTransactionTemplate _dbTransactionTemplate;
    private ILogger<TransactionsService> _logger;
    private UsersService _usersService;
    private CategoriesService _categoriesService;
    private readonly IMapper _mapper;
    public TransactionsService(AppDbContext appDbContext, DbTransactionTemplate dbTransactionTemplate,
        ILogger<TransactionsService> logger, UsersService usersService, IMapper mapper, CategoriesService categoriesService)
    {
        _appDbContext = appDbContext;
        _dbTransactionTemplate = dbTransactionTemplate;
        _logger = logger;
        _usersService = usersService;
        _mapper = mapper;
        _categoriesService = categoriesService;
    }

    // Db Transactions required due to changing user balance
    public async Task CreateTransactionAsync(TransactionDTO userTransactionDTO)
    {
        _logger.LogInformation("Executing CreateTransactionAsync method.");
        await _dbTransactionTemplate.ExecuteTransactionAsync(async () =>
        {                       
            var user = await _usersService.GetUserByIdAsync(userTransactionDTO.UserId);

            var transaction = _mapper.Map<UserTransaction>(userTransactionDTO);
            transaction.Category = await _categoriesService.GetCategoryByIdAsync(userTransactionDTO.CategoryId);

            await _appDbContext.Transactions.AddAsync(transaction);

            if (transaction.Category.IsIncome) user.Balance += transaction.Price;
            else user.Balance -= transaction.Price;

            await _appDbContext.SaveChangesAsync();

            userTransactionDTO.Id = transaction.Id;
        });
    }

    public async Task<UserTransaction> GetTransactionByIdAsync(int transactionId)
    {             
        var transaction = await _appDbContext.Transactions
            .Include(t => t.Category)
            .Include(t => t.InnerCategory)
            .FirstOrDefaultAsync(t => t.Id == transactionId);

        if (transaction == null) throw new EntityNotFoundException<UserTransaction>(transactionId);

        return transaction;
    }

    public async Task<TransactionDTO> GetTransactionDTOByIdAsync(int transactionId)
    {        
        var transaction = await _appDbContext.Transactions
            .FirstOrDefaultAsync(t => t.Id == transactionId);

        if (transaction == null) throw new EntityNotFoundException<UserTransaction>(transactionId);

        return _mapper.Map<TransactionDTO>(transaction);
    }


    public async Task<List<TransactionDTO>> GetTransactionsAsync(int userId, DateTime? lastDate, int pageSize)
    {      
        var orderedTransactions = _appDbContext.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date);

        if (lastDate.HasValue)
        {
            orderedTransactions = orderedTransactions
                .Where(t => t.Date<lastDate.Value)
                .OrderByDescending(t => t.Date);
        }

        var transactions = await orderedTransactions
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<List<TransactionDTO>>(transactions);
    }

    public async Task UpdateTransactionAsync(TransactionDTO newUserTransactionDTO)
    {
        _logger.LogInformation("Executing UpdateTransactionAsync method.");
        await _dbTransactionTemplate.ExecuteTransactionAsync(async () =>
        {
            var user = await _usersService.GetUserByIdAsync(newUserTransactionDTO.UserId);

            var oldUserTransaction = await GetTransactionByIdAsync(newUserTransactionDTO.Id);            

            if (newUserTransactionDTO.Price >= oldUserTransaction.Price)
            {
                user.Balance -= newUserTransactionDTO.Price - oldUserTransaction.Price;
            }
            else if (newUserTransactionDTO.Price <= oldUserTransaction.Price)
            {
                user.Balance += oldUserTransaction.Price - newUserTransactionDTO.Price;
            }

            oldUserTransaction.Name = newUserTransactionDTO.Name;
            oldUserTransaction.Price = newUserTransactionDTO.Price;
            oldUserTransaction.Date = newUserTransactionDTO.Date;

            oldUserTransaction.CategoryId = newUserTransactionDTO.CategoryId;

            if (newUserTransactionDTO.InnerCategoryId == 0) oldUserTransaction.InnerCategoryId = null;
            else oldUserTransaction.InnerCategoryId = newUserTransactionDTO.InnerCategoryId;

            //oldUserTransaction.Photo = newUserTransactionDTO.Photo;
        });
    }

    public async Task DeleteTransactionAsync(int transactionId)
    {
        _logger.LogInformation("Executing DeleteTransactionAsync method.");
        await _dbTransactionTemplate.ExecuteTransactionAsync(async () =>
        {            
            var transaction = await GetTransactionByIdAsync(transactionId);

            var user = await _usersService.GetUserByIdAsync(transaction.UserId);

            user.Balance += transaction.Price;

            _appDbContext.Transactions.Remove(transaction);
        });
    }    
}
