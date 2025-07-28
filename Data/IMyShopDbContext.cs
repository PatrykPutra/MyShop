using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.Data
{
    public interface IMyShopDbContext
    {
        DbSet<ExchangeRate> ExchangeRates { get; set; }
        DbSet<ItemCategory> ItemCategories { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<ShopItem> ShopItems { get; set; }
    }
}