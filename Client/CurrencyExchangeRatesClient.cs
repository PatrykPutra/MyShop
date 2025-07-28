using MyShop.Models;
using System.Text.Json;

namespace MyShop.Client
{
    public class CurrencyExchangeRatesClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private string _apiKey = "b10b189b0bbc43698ede82a25efa8079";
        private string _connectionString = "https://openexchangerates.org/api/latest.json?app_id=b10b189b0bbc43698ede82a25efa8079"; //move to appsettings

        public async Task<Dictionary<string,decimal>> GetExchangeRatesAsync()
        {
            var response = await _httpClient.GetAsync(_connectionString);
            response.EnsureSuccessStatusCode(); 

            var responseBody = await response.Content.ReadAsStringAsync();
            var exchangeRates = JsonSerializer.Deserialize<ExchangeRatesRoot>(responseBody);

            return exchangeRates.rates;
        }

        

    }
}
