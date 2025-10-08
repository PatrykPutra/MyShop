using MyShop.Models;
using System.Text.Json;


namespace MyShop.Client
{
    public class CurrencyExchangeRatesClient : ICurrencyExchangeRatesClient
    {
        private readonly IHttpClientFactory _factory;
        
        public CurrencyExchangeRatesClient(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync() 
        {
            var httpClient = _factory.CreateClient("exchangeRates");
            var response = await httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var exchangeRates = JsonSerializer.Deserialize<ExchangeRatesRoot>(responseBody,jsonOptions);
            if (exchangeRates == null) throw new Exception();
            List<ExchangeRateDto> rates = exchangeRates.Rates.Select(element => new ExchangeRateDto() { CurrencyCode = element.Key, Rate = element.Value }).ToList();

            return rates;
        }

        


    }
}
