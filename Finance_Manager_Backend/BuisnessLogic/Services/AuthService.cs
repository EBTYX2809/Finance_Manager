using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Finance_Manager_Backend.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Finance_Manager_Backend.BuisnessLogic.Models;

namespace Finance_Manager_Backend.BuisnessLogic.Services;

public class AuthService
{
    private readonly string credentialsPath = "appsettings.json";
    private readonly AppDbContext _dbContext;
    public AuthService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveCredentials(string email, string password)
    {
        var json = await File.ReadAllTextAsync(credentialsPath);
        var jsonObj = JObject.Parse(json);

        string encryptedPassword = EncryptPassword(password);

        jsonObj["UserCredentials"]["Email"] = email;
        jsonObj["UserCredentials"]["Password"] = encryptedPassword;

        await File.WriteAllTextAsync(credentialsPath, jsonObj.ToString());
    }

    public (string email, string password) LoadCredentials()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(credentialsPath, optional: false, reloadOnChange: true)
            .Build();        

        string email = config["UserCredentials:Email"] ?? "";
        string encryptedPassword = config["UserCredentials:Password"] ?? "";
        
        string password = DecryptPassword(encryptedPassword);

        return (email, password);
    }

    public async Task<User?> RegisterUserAsync(string email, string password)
    {
        var userCheck = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userCheck != null)
        {
            //MessageBox.Show("Error, this email already registered.");
            return null;
        }

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
            //MessageBox.Show("Error, user not found.");
            return null;
        }

        string hashedInput = HashPassword(password, user.Salt);

        if (user.PasswordHash == hashedInput) 
        {
            return user;
        }
        else
        {
            //MessageBox.Show("Invalid email or password. Please try again.");
            return null;
        }
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

    private string EncryptPassword(string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] encryptedBytes = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(encryptedBytes);
    }

    private string DecryptPassword(string encryptedPassword)
    {
        try
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
            byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        catch
        {
            return "";
        }
    }
}
