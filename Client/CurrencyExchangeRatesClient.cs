using MyShop.Models;
using System.Text.Json;

namespace MyShop.Client
{
    public class CurrencyExchangeRatesClient : ICurrencyExchangeRatesClient
    {
        private readonly HttpClient _httpClient = new HttpClient(); // W taki sposób się nie tworzy klienta. https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines
        private readonly IConfiguration _configuration; // Lepiej żebyś wstrzykiwał typed configuraiton a nie całe configuration
        public CurrencyExchangeRatesClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* Lepiej posługiwać się konkretnymi typami niż takimi które mają znaczenie umowne. Ja rozumiem że Dictionary<string, decimal> to jakiś słownik przeliczeń ale dużo lepiej by mi się potem na to w kodzie gdzieś patrzyło
            jakby to była  lista np. takiej klasy

            class ExchangeDto
           {
               public string CurrencyCode { get; set; }
               public decimal Rate { get; set; }
           }

        */
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
