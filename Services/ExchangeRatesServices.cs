using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Client;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Services
{
    public class ExchangeRatesServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly CurrencyExchangeRatesClient _exchangeRatesClient = new();
        public ExchangeRatesServices(MyShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task UpdateExchangeRatesAsync()
        {
            var exchangeRates = await _exchangeRatesClient.GetExchangeRatesAsync();

            var exchangeRatesKeys = exchangeRates.Keys;

            foreach (var key in exchangeRatesKeys)
            {
                ExchangeRate? exchangeRate = await _dbContext.ExchangeRates.FirstOrDefaultAsync(exchangeRate => exchangeRate.Name == key);
                if (exchangeRate == null)
                {
                    exchangeRate = new ExchangeRate()
                    {
                        Name = key,
                        Rate = exchangeRates[key],
                        Updated = DateTime.Today
                    };
                    _dbContext.ExchangeRates.Add(exchangeRate);
                }
                else
                {
                    exchangeRate.Rate = exchangeRates[key];
                    exchangeRate.Updated = DateTime.Today;
                }
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task<decimal> GetExchangeRateAsync(string currencyName)
        {
            if (!_dbContext.ExchangeRates.Any()) await UpdateExchangeRatesAsync();

            ExchangeRate? exchangeRate = await _dbContext.ExchangeRates.FirstOrDefaultAsync(exchangeRate => exchangeRate.Name == currencyName);

            if (exchangeRate == null) throw new ArgumentException("Given currecy name is not supported");
            if (exchangeRate.Updated < DateTime.Today) await UpdateExchangeRatesAsync();
            return exchangeRate.Rate;
        }

    }
}
