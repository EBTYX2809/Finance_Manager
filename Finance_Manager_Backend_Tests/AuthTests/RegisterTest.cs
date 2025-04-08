using Xunit;
using Finance_Manager_Backend.BuisnessLogic.Services;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend_Tests.AuthTests;

public class RegisterTest
{
    private readonly string email = "test@example.com";
    private readonly string password = "qwerty";

    [Fact]
    public async void RegisterUserInDataBase_Test()
    {
        // Arrange
        using var dbContext = TestDbContext.Create();

        var authSevice = new AuthService(dbContext);

        // Act
        await authSevice.RegisterUserAsync(email, password);
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
        Assert.NotNull(user.PasswordHash);
        Assert.NotNull(user.Salt);
    }

    [Fact]
    public async void RegisterUserInDataBaseWithExistedEmail_Test()
    {
        // Arrange
        using var dbContext = TestDbContext.Create();

        var authSevice = new AuthService(dbContext);

        // Act
        await authSevice.RegisterUserAsync(email, password);
        var user = await authSevice.RegisterUserAsync(email, "new password");

        // Assert
        Assert.Null(user);
    }
}
