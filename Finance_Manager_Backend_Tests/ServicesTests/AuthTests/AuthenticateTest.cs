using Xunit;
using Finance_Manager_Backend.BuisnessLogic.Services;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend_Tests.ServicesTests.AuthTests;

public class AuthenticateTest
{
    private readonly string email = "test@example.com";
    private readonly string password = "qwerty";

    [Fact]
    public async void AuthenticateUserFromDataBase_Test()
    {
        // Arrange
        using var dbContext = TestInMemoryDbContext.Create();

        var authSevice = new AuthService(dbContext);

        // Act
        await authSevice.RegisterUserAsync(email, password);
        var user = await authSevice.AuthenticateUserAsync(email, password);

        // Assert
        Assert.NotNull(user);        
        Assert.Equal(email, user.Email);
        Assert.NotNull(user.PasswordHash);
        Assert.NotNull(user.Salt);
        Console.WriteLine($"Generated password: {user.PasswordHash}. Salt: {user.Salt}.");
    }

    [Fact]
    public async void AuthenticateUserWithInvalidPasswordFromDataBase_Test()
    {
        // Arrange
        using var dbContext = TestInMemoryDbContext.Create();

        var authSevice = new AuthService(dbContext);

        // Act
        await authSevice.RegisterUserAsync(email, password);
        var user = await authSevice.AuthenticateUserAsync(email, "invalid");

        // Assert
        Assert.Null(user);
    }
}
