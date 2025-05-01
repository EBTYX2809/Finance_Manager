using System.Security.Cryptography;
using Finance_Manager_Backend.DataBase;
using Microsoft.EntityFrameworkCore;
using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Finance_Manager_Backend.Exceptions;

namespace Finance_Manager_Backend.BusinessLogic.Services.AuthServices;

public class AuthService
{
    private readonly AppDbContext _appDbContext;
    private readonly TokenGenerator _tokenGenerator;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    public AuthService(AppDbContext dbContext, TokenGenerator tokenGenerator, IMapper mapper, IMemoryCache cache)
    {
        _appDbContext = dbContext;
        _tokenGenerator = tokenGenerator;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<AuthUserTokensDTO> RegisterUserAsync(string email, string password)
    {
        var userCheck = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userCheck != null) throw new InvalidOperationException("Error, this email already registered.");

        string salt = GenerateSalt();
        string hashedPassword = HashPassword(password, salt);

        User user = new User(email, salt, hashedPassword, 0);
        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();

        // Saving user in cache for 1 hour
        var cacheKey = $"User_{user.Id}";
        _cache.Set(cacheKey, user, TimeSpan.FromHours(1));        

        var userDTO = _mapper.Map<UserDTO>(user);
        var accessToken = _tokenGenerator.GenerateAccessJwtToken(user);   
        var refreshToken = _tokenGenerator.GenerateRefreshToken(user);

        return new AuthUserTokensDTO { UserDTO = userDTO, AccessJwtToken = accessToken, RefreshToken = refreshToken };
    }

    public async Task RegisterAdminAsync(string email, string password)
    {
        var userCheck = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userCheck != null) throw new InvalidOperationException("Error, this email already registered.");

        string salt = GenerateSalt();
        string hashedPassword = HashPassword(password, salt);

        User user = new User(email, salt, hashedPassword, 100000) { Role = "Admin" };

        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<AuthUserTokensDTO> AuthenticateUserAsync(string email, string password)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) throw new InvalidOperationException("Error, this email not registered.");

        string hashedInput = HashPassword(password, user.Salt);

        if (user.PasswordHash == hashedInput) 
        {
            // Saving user in cache for 1 hour
            var cacheKey = $"User_{user.Id}";
            _cache.Set(cacheKey, user, TimeSpan.FromHours(1));

            var userDTO = _mapper.Map<UserDTO>(user);
            var accessToken = _tokenGenerator.GenerateAccessJwtToken(user);
            var refreshToken = _tokenGenerator.GenerateRefreshToken(user);

            return new AuthUserTokensDTO { UserDTO = userDTO, AccessJwtToken = accessToken, RefreshToken = refreshToken };
        }
        else throw new InvalidOperationException("Invalid password. Please try again.");
    }

    public async Task<AuthUserTokensDTO> AuthenticateUserWithRefreshTokenAsync(string refreshToken)
    {
        var existingToken = await _appDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if(existingToken == null) throw new InvalidOperationException("Invalid refresh token");
        else if (existingToken.ExpiresAt < DateTime.UtcNow) throw new InvalidOperationException("Expired refresh token");

        // Should use UsersService, but don't want load DI for 1 time using method.
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == existingToken.UserId);
        if (user == null) throw new EntityNotFoundException<User>(existingToken.UserId);

        existingToken.IsRevoked = true;
        await _appDbContext.SaveChangesAsync();

        var userDTO = _mapper.Map<UserDTO>(user);
        var newAaccessToken = _tokenGenerator.GenerateAccessJwtToken(user);
        var newRefreshToken = _tokenGenerator.GenerateRefreshToken(user);

        return new AuthUserTokensDTO { UserDTO = userDTO, AccessJwtToken = newAaccessToken, RefreshToken = newRefreshToken };
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
