using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend.BusinessLogic.Services;

public class UsersService
{
    private AppDbContext _appDbContext;
    public UsersService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) throw new EntityNotFoundException<User>(userId);

        return user;
    }
}
