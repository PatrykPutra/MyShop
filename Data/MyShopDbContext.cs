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
        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public MyShopDbContext(DbContextOptions<MyShopDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopItem>()
                .HasOne(shopItem => shopItem.Category)
                .WithMany(category => category.ShopItems)
                .HasForeignKey(shopItem => shopItem.CategoryId);

            modelBuilder.Entity<ShopItem>(entityBuilder =>
                entityBuilder.Property(shopItem => shopItem.PriceUSD).HasColumnType("decimal(7,2)"));

            modelBuilder.Entity<Order>(entityBuilder =>
            {
                entityBuilder.Property(order => order.TotalPriceUSD).HasColumnType("decimal(7,2)");
                entityBuilder.Property(order => order.CreationDate).HasColumnType("date");
            });
            
            modelBuilder.Entity<ItemCategory>()
                .HasData(new { Id = 1, Name = "Other" });

            modelBuilder.Entity<ExchangeRate>(entityBuilder =>
                entityBuilder.Property(exchangeRate => exchangeRate.Rate).HasColumnType("decimal(18,6"));
            modelBuilder.Entity<ExchangeRate>(entityBuilder =>
                entityBuilder.Property(exchangeRate => exchangeRate.Updated).HasColumnType("date"));

            modelBuilder.Entity<User>()
                .HasOne(user => user.Cart)
                .WithOne(shoppingCart => shoppingCart.User)
                .HasForeignKey<ShoppingCart>(shoppingCart => shoppingCart.UserId);
            
            modelBuilder.Entity<User>()
                .HasOne(user => user.Token)
                .WithOne(token => token.User)
                .HasForeignKey<Token>(token => token.UserId);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Orders)
                .WithOne(order => order.User)
                .HasForeignKey(order => order.UserId);

            modelBuilder.Entity<User>(entityBuilder =>
            {
                entityBuilder.Property(user => user.UserName).HasMaxLength(25);
                entityBuilder.Property(user => user.Email).HasMaxLength(255);
                
            });
        


        }

    }
}
