using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Client;
using MyShop.Data;
using MyShop.Exceptions;
using MyShop.Models;

namespace MyShop.Services
{
    public class ExchangeRatesServices : IExchangeRatesServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly ICurrencyExchangeRatesClient _exchangeRatesClient;
        public ExchangeRatesServices(MyShopDbContext dbContext, ICurrencyExchangeRatesClient exchangeRatesClient)
        {
            _dbContext = dbContext;
            _exchangeRatesClient = exchangeRatesClient;
        }

        private async Task UpdateExchangeRatesAsync()
        {
            var exchangeRates = await _exchangeRatesClient.GetExchangeRatesAsync();

            foreach (var rate in exchangeRates)
            {
                ExchangeRate? exchangeRate = _dbContext.ExchangeRates.FirstOrDefault(exchangeRate => exchangeRate.Name == rate.CurrencyCode);
                if (exchangeRate == null)
                {
                    exchangeRate = new ExchangeRate()
                    {
                        Name = rate.CurrencyCode,
                        Rate = rate.Rate,
                        Updated = DateTime.Today
                    };
                    _dbContext.ExchangeRates.Add(exchangeRate);
                }
                else
                {
                    exchangeRate.Rate = rate.Rate;
                    exchangeRate.Updated = DateTime.Today;
                }
            }
            _dbContext.SaveChanges();
        }

        public async Task<decimal> GetExchangeRateAsync(string currencyName)
        {
            if (!_dbContext.ExchangeRates.Any()) await UpdateExchangeRatesAsync();

            ExchangeRate? exchangeRate = await _dbContext.ExchangeRates.FirstOrDefaultAsync(exchangeRate => exchangeRate.Name == currencyName);

            if (exchangeRate == null) throw new NotFoundException($"Currecy name: {currencyName} not found");
            if (exchangeRate.Updated < DateTime.Today) await UpdateExchangeRatesAsync();
            return exchangeRate.Rate;
        }

    }
}
