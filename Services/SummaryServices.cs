using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Services
{
    public interface ISummaryServices
    {
        Task<Summary> GetAsync();
    }
    public class SummaryServices : ISummaryServices
    {
        private readonly MyShopDbContext _dbContext;
        public SummaryServices(MyShopDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<Summary> GetAsync()
        {
            int numberOfProducts = await _dbContext.ShopItems.CountAsync();
            int numberOfOrders = await _dbContext.Orders.CountAsync();
            int allProductsQuantity = await _dbContext.ShopItems.Select(shopItem => shopItem.Quantity).SumAsync();
            decimal allOrdersTotalPriceUSD = await _dbContext.Orders.Select(order => order.TotalPriceUSD).SumAsync();
            Summary summary = new()
            {
                NumberOfProducts = numberOfProducts,
                NumberOfOrders = numberOfOrders,
                AllProductsQuantity = allProductsQuantity,
                AllOrdersTotalPriceUSD = allOrdersTotalPriceUSD
            };
            return summary;
        }
    }
}
