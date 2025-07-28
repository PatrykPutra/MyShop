
namespace MyShop.Services
{
    public interface IExchangeRatesServices
    {
        Task<decimal> GetExchangeRateAsync(string currencyName);
    }
}