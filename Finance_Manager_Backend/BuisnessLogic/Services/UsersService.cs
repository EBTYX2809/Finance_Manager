using Finance_Manager_Backend.BuisnessLogic.Models;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend.BuisnessLogic.Services;

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

        if (user == null) throw new UserNotFoundException(userId.ToString());

        return user;
    }
}
