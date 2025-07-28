using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.Data
{
    public class MyShopDbContext : DbContext
    {
        public DbSet<ShopItem> ShopItems { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public MyShopDbContext(DbContextOptions<MyShopDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopItem>(entityBuilder =>
                entityBuilder.Property(shopItem => shopItem.PriceUSD).HasColumnType("decimal(7,2)"));
            modelBuilder.Entity<Order>(entityBuilder =>
                entityBuilder.Property(order => order.TotalPriceUSD).HasColumnType("decimal(7,2)"));
            modelBuilder.Entity<Order>(entityBuilder =>
                entityBuilder.Property(order => order.CreationDate).HasColumnType("date"));
            modelBuilder.Entity<ShopItem>()
                .HasOne(shopItem => shopItem.Category)
                .WithMany(category => category.ShopItems)
                .HasForeignKey(shopItem => shopItem.CategoryId);

            modelBuilder.Entity<ItemCategory>()
                .HasData(new { Id = 1, Name = "Other" });

            modelBuilder.Entity<ExchangeRate>(entityBuilder =>
                entityBuilder.Property(exchangeRate => exchangeRate.Rate).HasColumnType("decimal(18,6"));
            modelBuilder.Entity<ExchangeRate>(entityBuilder =>
                entityBuilder.Property(exchangeRate => exchangeRate.Updated).HasColumnType("date"));


        }

    }
}
