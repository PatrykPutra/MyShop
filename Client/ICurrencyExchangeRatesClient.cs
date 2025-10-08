
using MyShop.Models;

namespace MyShop.Client
{
    public interface ICurrencyExchangeRatesClient
    {
        Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync();
    }
}