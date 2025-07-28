using MyShop.Models;
using System.Text.Json;

namespace MyShop.Client
{
    public class CurrencyExchangeRatesClient : ICurrencyExchangeRatesClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;
        public CurrencyExchangeRatesClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync()
        {
            var response = await _httpClient.GetAsync(_configuration.GetConnectionString("exchangeRates"));
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var exchangeRates = JsonSerializer.Deserialize<ExchangeRatesRoot>(responseBody);

            return exchangeRates.rates;
        }



    }
}
