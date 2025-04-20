using AutoMapper;
using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.ModelsDTO;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend.BusinessLogic.Services;

public class AnalyticsService
{
    private AppDbContext _appDbContext;    
    private readonly UsersService _usersService;
    private readonly CategoriesService _categoriesService;
    private readonly IMapper _mapper;
    public AnalyticsService(AppDbContext appDbContext, UsersService usersService, CategoriesService categoriesService, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _usersService = usersService;
        _categoriesService = categoriesService;
        _mapper = mapper;
    }

    public async Task<Dictionary<CategoryDTO, float>> GetAlalyticFromDateAsync(int userId, DateTime minDate, DateTime maxDate)
    {
        var user = await _usersService.GetUserByIdAsync(userId);
        
        var spendsByCategory = await _appDbContext.Transactions                
            .Where(s => s.UserId == userId 
                     && !s.Category.IsIncome 
                     && s.Date > minDate 
                     && s.Date < maxDate)                      
            .GroupBy(s => s.Category)
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
        var user = await _usersService.GetUserByIdAsync(userId);

        var parentCategory = await _categoriesService.GetCategoryByIdAsync(parentCategoryId);

        var spendsByCategory = await _appDbContext.Transactions
            .Where(s => s.UserId == userId
                     && s.Category == parentCategory
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
