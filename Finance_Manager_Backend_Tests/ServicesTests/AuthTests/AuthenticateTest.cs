using Xunit;
using Finance_Manager_Backend.BusinessLogic.Services.AuthServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Finance_Manager_Backend.BusinessLogic.Models;
using Newtonsoft.Json.Linq;

namespace Finance_Manager_Backend_Tests.ServicesTests.AuthTests;

public class AuthenticateTest
{
    private readonly string email = "test@example.com";
    private readonly string password = "qwerty";
    private readonly JwtTokenGenerator tokenGenerator;

    public AuthenticateTest()
    {
        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Test.json")
        .Build();

        tokenGenerator = new JwtTokenGenerator(config);
    }

    [Fact]
    public async void AuthenticateUserFromDataBase_Test()
    {
        // Arrange
        using var dbContext = TestInMemoryDbContext.Create();

        var authSevice = new AuthService(dbContext, tokenGenerator);

        // Act
        var (User, Token) = await authSevice.RegisterUserAsync(email, password);
        var (user, token) = await authSevice.AuthenticateUserAsync(email, password);

        // Assert
        Assert.Equal(User, user);
        Assert.Equal(Token, token);        
    }

    [Fact]
    public async void AuthenticateUserWithInvalidPasswordFromDataBase_Test()
    {
        // Arrange
        using var dbContext = TestInMemoryDbContext.Create();

        var authSevice = new AuthService(dbContext, tokenGenerator);

        // Act
        var (User, Token) = await authSevice.RegisterUserAsync(email, password);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await authSevice.AuthenticateUserAsync(email, "invalid"));
    }
}
