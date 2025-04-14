using Finance_Manager_Backend.BuisnessLogic.Models;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend.BuisnessLogic.Services;

public class AnalyticsService
{
    private AppDbContext _appDbContext;    
    public AnalyticsService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;        
    }

    public async Task<Dictionary<Category, float>> GetAlalyticFromDate(int userId, DateTime minDate, DateTime maxDate)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) throw new EntityNotFoundException<User>(userId);
        
        var spendsByCategory = await _appDbContext.Transactions                
            .Where(s => s.UserId == userId 
                     && !s.Category.IsIncome 
                     && s.Date > minDate 
                     && s.Date < maxDate)                      
            .GroupBy(s => s.Category)
            .ToDictionaryAsync(g => g.Key, g => g.Sum(x => x.Price));

        decimal generalSpends = spendsByCategory.Values.Sum();
        Dictionary<Category, float> percentsByCategory = new();

        foreach(var s in spendsByCategory)
        {
            percentsByCategory.Add(s.Key, (float)(s.Value / generalSpends * 100));
        }

        return percentsByCategory;
    }

    public async Task<Dictionary<Category, float>> GetInnerAlalyticFromDate(int userId, Category parentCategory, DateTime minDate, DateTime maxDate)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) throw new EntityNotFoundException<User>(userId);

        var spendsByCategory = await _appDbContext.Transactions
            .Where(s => s.UserId == userId
                     && s.Category == parentCategory
                     && s.Date > minDate
                     && s.Date < maxDate)
            .GroupBy(s => s.InnerCategory ?? new Category()) // ?? new Category() suppress null alerts.
            .ToDictionaryAsync(g => g.Key, g => g.Sum(x => x.Price));

        decimal generalSpends = spendsByCategory.Values.Sum();
        Dictionary<Category, float> percentsByCategory = new();

        foreach (var s in spendsByCategory)
        {
            percentsByCategory.Add(s.Key, (float)(s.Value / generalSpends * 100));
        }

        return percentsByCategory;
    }
}
