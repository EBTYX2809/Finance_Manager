using AutoMapper;
using Finance_Manager_Backend.BusinessLogic.Services.AuthServices;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend_Tests.ServicesTests.AuthTests;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Finance_Manager_Tests.ServicesTests.AuthTests;

public class AuthenticateUserWithRefreshTokenAsyncTest
{
    private readonly string email = "test@example.com";
    private readonly string password = "qwerty";
    private readonly TokenGenerator _tokenGenerator;
    private readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;
    private readonly AuthService _authService;

    public AuthenticateUserWithRefreshTokenAsyncTest()
    {
        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Test.json")
        .Build();

        _appDbContext = TestInMemoryDbContext.Create();
        _tokenGenerator = new TokenGenerator(config, _appDbContext);
        _mapper = AutoMapperFotTests.GetMapper();
        _authService = new AuthService(_appDbContext, _tokenGenerator, _mapper, new MemoryCache(new MemoryCacheOptions()));
    }

    [Fact]
    public async Task AuthenticateUserWithRefreshTokenAsyncWithValidToken_Test()
    {
        // Arrange
        var userDTO = await _authService.RegisterUserAsync(email, password);

        // Act
        var result = await _authService.AuthenticateUserWithRefreshTokenAsync(userDTO.RefreshToken);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.AccessJwtToken);
        Assert.NotNull(result.RefreshToken);
        Assert.Equal(userDTO.UserDTO.Email, result.UserDTO.Email);
        Assert.True(_appDbContext.RefreshTokens.First().IsRevoked);
    }

    [Fact]
    public async Task AuthenticateUserWithRefreshTokenAsyncWithInvalidToken_Test()
    {
        // Arrange
        var userDTO = await _authService.RegisterUserAsync(email, password);

        // Act && Assert              
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _authService.AuthenticateUserWithRefreshTokenAsync("invalid token"));
    }
}
