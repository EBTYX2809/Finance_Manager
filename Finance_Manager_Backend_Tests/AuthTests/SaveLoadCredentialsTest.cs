using Finance_Manager_Backend.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Finance_Manager_Backend_Tests.AuthTests;

public class SaveLoadCredentialsTest : IDisposable
{
    private readonly string credentialsPath = "appsettings.json";
    private readonly string email = "test@example.com";
    private readonly string password = "qwerty";

    [Fact]
    public async Task SaveCredentials_Test()
    {
        // Arrange 
        var authServise = new AuthService(null);

        // Act
        await authServise.SaveCredentials(email, password);

        var config = new ConfigurationBuilder()
            .AddJsonFile(credentialsPath, optional: false, reloadOnChange: true)
            .Build();

        string savedEmail = config["UserCredentials:Email"];
        string savedEncryptedPassword = config["UserCredentials:Password"];

        // Assert
        Assert.Equal(email, savedEmail);
        Assert.NotNull(savedEncryptedPassword);
    }

    [Fact]
    public async Task LoadCredentials_Test()
    {
        // Arrange 
        var authServise = new AuthService(null);                     

        // Act
        await authServise.SaveCredentials(email, password);

        var (savedEmail, savedDecryptedPassword) = authServise.LoadCredentials();                       

        // Assert
        Assert.Equal(email, savedEmail);
        Assert.Equal(password, savedDecryptedPassword);
    }

    public void Dispose()
    {
        string json = File.ReadAllText(credentialsPath);
        var jsonObj = JObject.Parse(json);

        var jsonEmail = jsonObj["UserCredentials"]["Email"];
        if (jsonEmail.ToString() != "") jsonEmail = string.Empty;

        var jsonPassword = jsonObj["UserCredentials"]["Password"];
        if (jsonPassword.ToString() != "") jsonPassword = string.Empty;

        File.WriteAllText(credentialsPath, jsonObj.ToString());
    }
}
