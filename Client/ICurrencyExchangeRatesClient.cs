
namespace MyShop.Client
{
    public interface ICurrencyExchangeRatesClient
    {
        Task<Dictionary<string, decimal>> GetExchangeRatesAsync();
    }
}