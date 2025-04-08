using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Finance_Manager_Backend.DataBase;

public class DbTransactionTemplate
{
    private AppDbContext _appDbContext;
    private ILogger<DbTransactionTemplate> _logger;
    public DbTransactionTemplate(AppDbContext appDbContext, ILogger<DbTransactionTemplate> logger)
    {
        _appDbContext = appDbContext;
        _logger = logger;
    }

    public async Task ExecuteTransactionAsync(Func<Task> transaction)
    {
        _logger.LogInformation("Start ExecuteTransactionAsync method.");
        int attempt = 0;
        int maxTransactionAttempts = 3;

        while (attempt < maxTransactionAttempts)
        {
            attempt++;

            using (var dbTransaction = await _appDbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                _logger.LogDebug("Start db transaction.");
                try
                {
                    await transaction.Invoke();

                    await _appDbContext.SaveChangesAsync();

                    await dbTransaction.CommitAsync();

                    _logger.LogInformation("Transaction successful.");
                    return;
                }
                catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("deadlock") == true)
                {
                    _logger.LogWarning("Deadlock, transaction failed. " +
                        "Try attempt {attempt}/{maxTransactionAttempts}.", attempt, maxTransactionAttempts);
                    await dbTransaction.RollbackAsync();
                    await Task.Delay(100 * attempt);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed transaction: {ex}", ex.Message);
                    await dbTransaction.RollbackAsync();
                    return;
                }
            }
        }

        _logger.LogWarning("Transaction failed after {maxAttempts} attempts.", maxTransactionAttempts);
    }
}
