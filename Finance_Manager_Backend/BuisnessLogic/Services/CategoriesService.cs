using Finance_Manager_Backend.BuisnessLogic.Models;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend.BuisnessLogic.Services;

public class CategoriesService
{
    private AppDbContext _appDbContext;
    public CategoriesService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        var category = await _appDbContext.Categories.FirstOrDefaultAsync(t => t.Id == categoryId);

        if (category == null) throw new EntityNotFoundException<Category>(categoryId);

        return category;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _appDbContext.Categories.ToListAsync();
    }
}
