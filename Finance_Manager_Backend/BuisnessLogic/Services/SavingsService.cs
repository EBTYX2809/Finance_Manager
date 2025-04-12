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
    public SavingsService(AppDbContext appDbContext, DbTransactionTemplate dbTransactionTemplate, ILogger<SavingsService> logger)
    {
        _appDbContext = appDbContext;
        _transactionTemplate = dbTransactionTemplate;
        _logger = logger;
    }

    public async Task CreateSavingAsync(Saving saving)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == saving.UserId);

        if (user == null) throw new UserNotFoundException(saving.UserId.ToString());

        await _appDbContext.Savings.AddAsync(saving);

        await _appDbContext.SaveChangesAsync();
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
            var saving = await _appDbContext.Savings.FirstOrDefaultAsync(s => s.Id == savingId);

            if (saving == null) throw new SavingIsNotExistException(savingId.ToString());

            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == saving.UserId);

            if (user == null) throw new UserNotFoundException(saving.UserId.ToString());

            saving.CurrentAmount += topUpAmount;
            user.Balance -= topUpAmount;
        });
    }

    public async Task DeleteSavingAsync(int savingId)
    {
        _logger.LogInformation("Executing DeleteSavingAsync method.");
        await _transactionTemplate.ExecuteTransactionAsync(async () =>
        {
            var saving = await _appDbContext.Savings.FirstOrDefaultAsync(s => s.Id == savingId);

            if (saving == null) throw new SavingIsNotExistException(savingId.ToString());

            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == saving.UserId);

            if (user == null) throw new UserNotFoundException(saving.UserId.ToString());

            user.Balance += saving.CurrentAmount;

            _appDbContext.Savings.Remove(saving);
        });
    }
}
