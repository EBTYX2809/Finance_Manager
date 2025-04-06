using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Finance_Manager_Backend.DataBase;

public class DbTransactionTemplate
{
    private AppDbContext _appDbContext;
    public DbTransactionTemplate(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task ExecuteTransactionAsync(Func<Task> transaction)
    {
        int attempt = 0;
        int maxTransactionAttempts = 3;

        while (attempt < maxTransactionAttempts)
        {
            attempt++;

            using (var dbTransaction = await _appDbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                try
                {
                    await transaction.Invoke();

                    await _appDbContext.SaveChangesAsync();

                    await dbTransaction.CommitAsync();

                    // Logger here:
                    Console.WriteLine("Transaction succesfull.");
                    return;
                }
                catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("deadlock") == true)
                {
                    // Logger here:
                    Console.WriteLine($"Deadlock, transaction failed. Try {attempt} attempt from {maxTransactionAttempts}.");
                    await dbTransaction.RollbackAsync();
                    await Task.Delay(100 * attempt);
                }
                catch (Exception ex)
                {
                    // Logger here:
                    Console.WriteLine($"Failed transaction {ex.Message}.");
                    await dbTransaction.RollbackAsync();
                    return;
                }
            }
        }

        // Logger here:
        Console.WriteLine($"Transaction failed after {maxTransactionAttempts} attempts.");
    }
}
