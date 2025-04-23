using AutoMapper;
using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend.BusinessLogic.Services;

public class AnalyticsService
{
    private AppDbContext _appDbContext;            
    private readonly IMapper _mapper;
    public AnalyticsService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;                
        _mapper = mapper;
    }

    public async Task<Dictionary<CategoryDTO, float>> GetAlalyticFromDateAsync(int userId, DateTime minDate, DateTime maxDate)
    {
        var spendsByCategory = await _appDbContext.Transactions
            .Where(s => s.UserId == userId
                     && !s.Category.IsIncome
                     && s.Date > minDate
                     && s.Date < maxDate)
            .GroupBy(s => s.Category) // ?? new Category() suppress null alerts.
            .ToDictionaryAsync(g => g.Key, g => g.Sum(x => x.Price));

        decimal generalSpends = spendsByCategory.Values.Sum();
        Dictionary<CategoryDTO, float> percentsByCategory = new();

        foreach(var s in spendsByCategory)
        {
            var category = _mapper.Map<CategoryDTO>(s.Key);
            var percent = (float)(s.Value / generalSpends * 100);
            percentsByCategory.Add(category, percent);
        }

        return percentsByCategory;
    }

    public async Task<Dictionary<CategoryDTO, float>> GetInnerAlalyticFromDateAsync(int userId, int parentCategoryId, DateTime minDate, DateTime maxDate)
    {
        var spendsByCategory = await _appDbContext.Transactions
            .Where(s => s.UserId == userId
                     && s.CategoryId == parentCategoryId
                     && s.Date > minDate
                     && s.Date < maxDate)
            .GroupBy(s => s.InnerCategory ?? new Category()) // ?? new Category() suppress null alerts.
            .ToDictionaryAsync(g => g.Key, g => g.Sum(x => x.Price));

        decimal generalSpends = spendsByCategory.Values.Sum();
        Dictionary<CategoryDTO, float> percentsByCategory = new();

        foreach (var s in spendsByCategory)
        {
            var category = _mapper.Map<CategoryDTO>(s.Key);
            var percent = (float)(s.Value / generalSpends * 100);
            percentsByCategory.Add(category, percent);
        }

        return percentsByCategory;
    }
}
