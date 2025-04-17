using System.Security.Cryptography;
using Finance_Manager_Backend.DataBase;
using Microsoft.EntityFrameworkCore;
using Finance_Manager_Backend.BusinessLogic.Models;

namespace Finance_Manager_Backend.BusinessLogic.Services.AuthServices;

public class AuthService
{
    private readonly AppDbContext _dbContext;
    private readonly JwtTokenGenerator _tokenGenerator;
    public AuthService(AppDbContext dbContext, JwtTokenGenerator tokenGenerator)
    {
        _dbContext = dbContext;
        _tokenGenerator = tokenGenerator;
    }   

    public async Task<(User, string)> RegisterUserAsync(string email, string password)
    {
        var userCheck = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userCheck != null) throw new InvalidOperationException("Error, this email already registered.");

        string salt = GenerateSalt();
        string hashedPassword = HashPassword(password, salt);

        User user = new User(email, salt, hashedPassword, 0);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();        

        return (user, _tokenGenerator.GenerateToken());
    }

    public async Task<(User, string)> AuthenticateUserAsync(string email, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) throw new InvalidOperationException("Error, this email not registered.");

        string hashedInput = HashPassword(password, user.Salt);

        if (user.PasswordHash == hashedInput) 
        {
            return (user, _tokenGenerator.GenerateToken());
        }
        else throw new InvalidOperationException("Invalid password. Please try again.");
    }

    private string HashPassword(string password, string salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password.Trim(), Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256))
        {
            return Convert.ToBase64String(pbkdf2.GetBytes(32));
        }
    }

    private string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }
}
