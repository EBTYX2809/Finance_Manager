using Finance_Manager_Backend.BusinessLogic.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Finance_Manager_Tests.ServicesTests;

public class CurrencyConverterServiceTests
{
    private readonly CurrencyConverterService _converter;    
    public CurrencyConverterServiceTests()
    {
        HttpClient client = new HttpClient();

        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Test.json")
        .Build();

        _converter = new CurrencyConverterService(client, config, new MemoryCache(new MemoryCacheOptions()));
    }

    [Fact]
    public async Task Converter_Test()
    {
        // Arrange
        string fromCurrency = "USD";
        string toCurrency = "UAH";
        decimal amount = 100;
        decimal realCourse = 41.79m;

        // Act
        decimal APIresult = (decimal)await _converter.ConvertAsync(amount, fromCurrency, toCurrency);
        decimal TESTresult = amount * realCourse;

        // Assert
        Assert.InRange(APIresult, TESTresult - 100, TESTresult + 100);
        Console.WriteLine($"Expected: {TESTresult}. Got: {APIresult}");
    }
}
