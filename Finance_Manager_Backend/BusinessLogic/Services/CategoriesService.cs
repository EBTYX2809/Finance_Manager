using AutoMapper;
using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.ModelsDTO;
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
}
