using AutoMapper;
using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend.BusinessLogic.Services;

public class CategoriesService
{
    private AppDbContext _appDbContext;
    private readonly IMapper _mapper;   
    public CategoriesService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        var category = await _appDbContext.Categories.FirstOrDefaultAsync(t => t.Id == categoryId);

        if (category == null) throw new EntityNotFoundException<Category>(categoryId);

        return category;
    }

    public async Task<CategoryDTO> GetCategoryDTOByIdAsync(int categoryId)
    {
        var category = await _appDbContext.Categories.FirstOrDefaultAsync(t => t.Id == categoryId);

        if (category == null) throw new EntityNotFoundException<Category>(categoryId);

        return _mapper.Map<CategoryDTO>(category);
    }

    public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
    {
        var categories = await _appDbContext.Categories.ToListAsync();

        return _mapper.Map<List<CategoryDTO>>(categories);
    }

    // Admin methods
    public async Task CreateAllCategoriesAsync()
    {
        await DataSeeder.SeedCategories(_appDbContext);
    }

    public async Task CreateNewCategoryAsync(CategoryDTO categoryDTO)
    {
        if (categoryDTO.ParentCategoryId != null && categoryDTO.ParentCategoryId != 0) // ParentCategoryId validation
        {
            var existParentCategory = await GetCategoryByIdAsync((int)categoryDTO.ParentCategoryId);
        }

        var category = _mapper.Map<Category>(categoryDTO);

        await _appDbContext.AddAsync(category);
        await _appDbContext.SaveChangesAsync();

        categoryDTO.Id = category.Id;
    }
    
    public async Task UpdateCategoryAsync(CategoryDTO categoryDTO)
    {
        var oldCategory = await GetCategoryByIdAsync(categoryDTO.Id);

        oldCategory.Name = categoryDTO.Name;
        oldCategory.Icon = oldCategory.Icon;
        oldCategory.ColorForBackground = categoryDTO.ColorForBackground;
        oldCategory.IsIncome = categoryDTO.IsIncome;

        if (categoryDTO.ParentCategoryId != null && categoryDTO.ParentCategoryId != 0) // ParentCategoryId validation
        {
            var existParentCategory = await GetCategoryByIdAsync((int)categoryDTO.ParentCategoryId);
        }

        if (categoryDTO.ParentCategoryId == 0) oldCategory.ParentCategoryId = null;
        else oldCategory.ParentCategoryId = categoryDTO.ParentCategoryId;

        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await GetCategoryByIdAsync(id);

        _appDbContext.Categories.Remove(category);

        await _appDbContext.SaveChangesAsync();
    }
}
