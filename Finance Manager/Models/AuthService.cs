using System.Security.Cryptography;
using System.Windows;
using Finance_Manager.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager.Models;

public class AuthService
{
    private readonly AppDbContext _dbContext;
    public AuthService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> RegisterUserAsync(string email, string password)
    {
        string salt = GenerateSalt();
        string hashedPassword = HashPassword(password, salt);

        User user = new User { Email = email, PasswordHash = hashedPassword, Salt = salt };
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            MessageBox.Show("Error, user not found.");
            return null;
        }

        string hashedInput = HashPassword(password, user.Salt);

        if (user.PasswordHash == hashedInput) 
        {
            return user;
        }
        else
        {
            MessageBox.Show("Invalid email or password. Please try again.");
            return null;
        }
    }

    private string HashPassword(string password, string salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256))
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
