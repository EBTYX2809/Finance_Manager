using Xunit;
using Finance_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Tests.AuthTests;

public class RegisterTest
{
    [Fact]
    public async void RegisterUserInDataBase_Test()
    {
        // Arrange
        using var dbContext = TestDbContext.Create();

        var authSevice = new AuthService(dbContext);

        string email = "test@example.com";
        string password = "qwerty";

        // Act
        await authSevice.RegisterUserAsync(email, password);
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
        Assert.NotNull(user.PasswordHash);
        Assert.NotNull(user.Salt);
    }
}
