using System.Security.Cryptography;
using Finance_Manager_Backend.DataBase;
using Microsoft.EntityFrameworkCore;
using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;

namespace Finance_Manager_Backend.BusinessLogic.Services.AuthServices;

public class AuthService
{
    private readonly AppDbContext _dbContext;
    private readonly JwtTokenGenerator _tokenGenerator;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    public AuthService(AppDbContext dbContext, JwtTokenGenerator tokenGenerator, IMapper mapper, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _tokenGenerator = tokenGenerator;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<(UserDTO, string)> RegisterUserAsync(string email, string password)
    {
        var userCheck = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userCheck != null) throw new InvalidOperationException("Error, this email already registered.");

        string salt = GenerateSalt();
        string hashedPassword = HashPassword(password, salt);

        User user = new User(email, salt, hashedPassword, 0);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        // Saving user in cache for 1 hour
        var cacheKey = $"User_{user.Id}";
        _cache.Set(cacheKey, user, TimeSpan.FromHours(1));

        var userDTO = _mapper.Map<UserDTO>(user);
        var token = _tokenGenerator.GenerateToken(user);

        return (userDTO, token);
    }

    public async Task RegisterAdminAsync(string email, string password)
    {
        var userCheck = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userCheck != null) throw new InvalidOperationException("Error, this email already registered.");

        string salt = GenerateSalt();
        string hashedPassword = HashPassword(password, salt);

        User user = new User(email, salt, hashedPassword, 100000) { Role = "Admin" };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(UserDTO, string)> AuthenticateUserAsync(string email, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) throw new InvalidOperationException("Error, this email not registered.");

        string hashedInput = HashPassword(password, user.Salt);

        if (user.PasswordHash == hashedInput) 
        {
            // Saving user in cache for 1 hour
            var cacheKey = $"User_{user.Id}";
            _cache.Set(cacheKey, user, TimeSpan.FromHours(1));

            var userDTO = _mapper.Map<UserDTO>(user);
            var token = _tokenGenerator.GenerateToken(user);

            return (userDTO, token);
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
