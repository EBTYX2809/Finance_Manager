using Xunit;
using Finance_Manager_Backend.BusinessLogic.Services.AuthServices;
using Microsoft.EntityFrameworkCore;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Finance_Manager_Tests.ServicesTests;

namespace Finance_Manager_Backend_Tests.ServicesTests.AuthTests;

public class RegisterTest
{
    private readonly string email = "test@example.com";
    private readonly string password = "qwerty";
    private readonly JwtTokenGenerator tokenGenerator;
    private readonly IMapper _mapper;

    public RegisterTest()
    {
        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Test.json")
        .Build();

        tokenGenerator = new JwtTokenGenerator(config);
        _mapper = AutoMapperFotTests.GetMapper();
    }

    [Fact]
    public async void RegisterUserInDataBase_Test()
    {
        // Arrange
        using var dbContext = TestInMemoryDbContext.Create();

        var authSevice = new AuthService(dbContext, tokenGenerator, _mapper);

        // Act
        var (User, Token) = await authSevice.RegisterUserAsync(email, password);
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

        // Assert
        Assert.NotNull(User);
        Assert.NotNull(Token);
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
        using var dbContext = TestInMemoryDbContext.Create();

        var authSevice = new AuthService(dbContext, tokenGenerator, _mapper);

        // Act
        var (User, Token) = await authSevice.RegisterUserAsync(email, password);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await authSevice.RegisterUserAsync(email, "new password"));
    }
}
