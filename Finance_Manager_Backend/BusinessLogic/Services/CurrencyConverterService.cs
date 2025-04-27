using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Finance_Manager_Backend.BusinessLogic.Services;

public class CurrencyConverterService
{
    private readonly HttpClient _client;
    private IConfiguration _configuration;
    private readonly IMemoryCache _cache;
    private string apiKey;
    private string url;
    public CurrencyConverterService(HttpClient client, IConfiguration configuration, IMemoryCache cache)
    {
        _client = client;
        _configuration = configuration;
        apiKey = _configuration["ExchangeRateAPI:APIKey"];
        url = _configuration["ExchangeRateAPI:URL"];
        _cache = cache;
    }
    public async Task<decimal?> Convert(decimal amount, string from, string to)
    {    
        if(string.IsNullOrWhiteSpace(to)) return null;

        string cacheKeyCurrencyPair = $"{from}-{to}";

        if(_cache.TryGetValue(cacheKeyCurrencyPair, out decimal cachedConversionRate))
        {
            return amount * cachedConversionRate;
        }

        var response = await _client.GetAsync($"{url}/{apiKey}/pair/{from}/{to}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed API request. StatusCode: {response.StatusCode}, Response: {error}");
        }

        string json = await response.Content.ReadAsStringAsync();

        ApiResponse data = JsonSerializer.Deserialize<ApiResponse>(json);

        if (data == null) throw new InvalidOperationException("Invalid API response.");

        _cache.Set(cacheKeyCurrencyPair, data.conversion_rate, 
            new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7) });

        return amount * data.conversion_rate;
    }
}

class ApiResponse
{
    public string base_code { get; set; } = string.Empty;
    public string target_code { get; set; } = string.Empty;
    public decimal conversion_rate { get; set; }
}
