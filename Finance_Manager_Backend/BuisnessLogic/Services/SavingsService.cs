using Finance_Manager_Backend.BuisnessLogic.Models;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend.BuisnessLogic.Services;

public class SavingsService
{
    private AppDbContext _appDbContext;
    private DbTransactionTemplate _transactionTemplate;
    private ILogger<SavingsService> _logger;
    private UsersService _usersService;
    public SavingsService(AppDbContext appDbContext, DbTransactionTemplate dbTransactionTemplate,
        ILogger<SavingsService> logger, UsersService usersService)
    {
        _appDbContext = appDbContext;
        _transactionTemplate = dbTransactionTemplate;
        _logger = logger;
        _usersService = usersService;
    }

    public async Task CreateSavingAsync(Saving saving)
    {
        var user = await _usersService.GetUserByIdAsync(saving.UserId);

        await _appDbContext.Savings.AddAsync(saving);

        await _appDbContext.SaveChangesAsync();
    }

    public async Task<Saving> GetSavingByIdAsync(int savingId)
    {
        var saving = await _appDbContext.Savings.FirstOrDefaultAsync(s => s.Id == savingId);

        if (saving == null) throw new SavingIsNotExistException(savingId.ToString());

        return saving;
    }

    public async Task<List<Saving>> GetSavingsAsync(int userId, int previousSavingId, int pageSize)
    {
        var savings = _appDbContext.Savings.Where(s => s.UserId == userId && s.Id > previousSavingId);

        return await savings
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task UpdateSavingAsync(int savingId, decimal topUpAmount)
    {
        _logger.LogInformation("Executing UdateSavingAsync method");
        await _transactionTemplate.ExecuteTransactionAsync(async () =>
        {
            var saving = await GetSavingByIdAsync(savingId);

            var user = await _usersService.GetUserByIdAsync(saving.UserId);

            saving.CurrentAmount += topUpAmount;
            user.Balance -= topUpAmount;
        });
    }

    public async Task DeleteSavingAsync(int savingId)
    {
        _logger.LogInformation("Executing DeleteSavingAsync method.");
        await _transactionTemplate.ExecuteTransactionAsync(async () =>
        {
            var saving = await GetSavingByIdAsync(savingId);

            var user = await _usersService.GetUserByIdAsync(saving.UserId);

            user.Balance += saving.CurrentAmount;

            _appDbContext.Savings.Remove(saving);
        });
    }
}
