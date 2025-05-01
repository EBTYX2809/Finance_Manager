using Xunit;
using Finance_Manager_Backend.BusinessLogic.Services.AuthServices;
using Microsoft.EntityFrameworkCore;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Finance_Manager_Tests.ServicesTests;
using Finance_Manager_Backend.DataBase;
using Microsoft.Extensions.Caching.Memory;

namespace Finance_Manager_Backend_Tests.ServicesTests.AuthTests;

public class RegisterTest
{
    private readonly string email = "test@example.com";
    private readonly string password = "qwerty";
    private readonly TokenGenerator _tokenGenerator;
    private readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;
    private readonly AuthService _authService;

    public RegisterTest()
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
    public async void RegisterUserInDataBase_Test()
    {
        // Arrange

        // Act
        var userDTO = await _authService.RegisterUserAsync(email, password);
        var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

        // Assert
        Assert.NotNull(userDTO);        
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
        Assert.NotNull(user.PasswordHash);
        Assert.NotNull(user.Salt);
        Console.WriteLine($"Generated password: {user.PasswordHash}. Salt: {user.Salt}.");
    }

    [Fact]
    public async void RegisterUserInDataBaseWithExistedEmail_Test()
    {
        // Arrange

        // Act
        var userDTO = await _authService.RegisterUserAsync(email, password);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await _authService.RegisterUserAsync(email, "new password"));
    }
}
