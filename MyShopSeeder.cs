using Microsoft.AspNetCore.Identity;
using MyShop.Data;
using MyShop.Entities;

namespace MyShop
{
    public class MyShopSeeder
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        public MyShopSeeder(MyShopDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (_dbContext.ItemCategories.Count()<=1)
                {
                    var itemCategories = GetItemCategories();
                    _dbContext.ItemCategories.AddRange(itemCategories);
                    _dbContext.SaveChanges();

                    if (!_dbContext.ShopItems.Any())
                    {
                        var shopItems = GetShopItems(itemCategories);
                        _dbContext.ShopItems.AddRange(shopItems);
                        _dbContext.SaveChanges();
                    }
                }
                if (!_dbContext.OrderStatuses.Any())
                {
                    var orderStatuses = GetOrderStatuses();
                    _dbContext.OrderStatuses.AddRange(orderStatuses);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Users.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<ItemCategory> GetItemCategories()
        {
            return new List<ItemCategory>()
            {

                new ItemCategory()
                {
                    Name = "Shirts"
                },
                new ItemCategory()
                {
                    Name = "Trousers"
                },
                new ItemCategory()
                {
                    Name = "Footwear"
                },
                new ItemCategory()
                {
                    Name = "Jackets"
                },
                new ItemCategory()
                {
                    Name = "Dresses"
                },
                new ItemCategory()
                {
                    Name = "Underwear"
                }
            };
        }
        private IEnumerable<ShopItem> GetShopItems(IEnumerable<ItemCategory> itemCategories)
        {
            return new List<ShopItem>()
            {
                new ShopItem()
                {
                    Name = "Blue Short Sleeve Men's T-Shirt",
                    Text ="The perfect tee shirt for a modern casual look. Not too long, so you can wear these untucked with a pair of jeans or chinos.",
                    PriceUSD = 5,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Shirts").Id,
                    Category =itemCategories.First(category=>category.Name =="Shirts")
                },
                new ShopItem()
                {
                    Name = "Green Short Sleeve Men's T-Shirt",
                    Text ="The perfect tee shirt for a modern casual look. Not too long, so you can wear these untucked with a pair of jeans or chinos.",
                    PriceUSD = 5,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Shirts").Id,
                    Category =itemCategories.First(category=>category.Name =="Shirts")
                },
                new ShopItem()
                {
                    Name = "Mens Cotton Crew Neck Short Sleeve T-Shirt",
                    Text ="Made of 100% soft cotton for a smooth, breathable fit.",
                    PriceUSD = 5,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Shirts").Id,
                    Category =itemCategories.First(category=>category.Name =="Shirts")
                },
                new ShopItem()
                {
                    Name = "Men's 100% Cotton Regular Fit Jeans",
                    Text = "100% Cotton for durability and comfort",
                    PriceUSD = 33,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Trousers").Id,
                    Category =itemCategories.First(category=>category.Name =="Trousers")
                },
                new ShopItem()
                {
                    Name = "Men's Tall Relaxed Fit Jeans",
                    Text = "Designed for all-day comfort with a relaxed fit through the seat and thigh.",
                    PriceUSD = 26,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Trousers").Id,
                    Category =itemCategories.First(category=>category.Name =="Trousers")
                },
                new ShopItem()
                {
                    Name = "Men's Skinny Fit Stretch Jeans",
                    Text = "Perfect for the guy who's on top of his style game.",
                    PriceUSD = 22,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Trousers").Id,
                    Category =itemCategories.First(category=>category.Name =="Trousers")
                },
                new ShopItem()
                {
                    Name = "Athletic Performance Running Shoes",
                    Text = "Running Sneakers for Men for active look and everyday lifestyle.",
                    PriceUSD = 26,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Footwear").Id,
                    Category =itemCategories.First(category=>category.Name =="Footwear")
                },
                new ShopItem()
                {
                    Name = "Men's Slip-on Sneaker ",
                    Text = "This everyday trail design features a leather and mesh upper with a stretch lace panel.",
                    PriceUSD = 20,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Footwear").Id,
                    Category =itemCategories.First(category=>category.Name =="Footwear")
                },
                new ShopItem()
                {
                    Name = "Men's Benny Boat Shoes",
                    Text = "Benny boat shoe., with the classic look of northeast nautical, including comfort, traction and durability features he will need for smooth sailing.",
                    PriceUSD = 35,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Footwear").Id,
                    Category =itemCategories.First(category=>category.Name =="Footwear")
                },
                new ShopItem()
                {
                    Name = "Men's Canvas Barn Jacket",
                    Text = "Features a cotton canvas shell, contrast corduroy collar for a pop of style and plaid flannel lining to help keep you warm.",
                    PriceUSD = 40,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Jackets").Id,
                    Category =itemCategories.First(category=>category.Name =="Jackets")
                },
                new ShopItem()
                {
                    Name = "Men Outdoor Fleece Lining Print Jacket",
                    Text = "These mens sherpa hoodies offer complete warmth, making them an ideal gift for those naturally sensitive to the cold or seeking comfort in chilly weather, including seniors",
                    PriceUSD = 42,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Jackets").Id,
                    Category =itemCategories.First(category=>category.Name =="Jackets")
                },
                new ShopItem()
                {
                    Name = "Men's Shirt Jacket with Hood",
                    Text = "This men’s shacket offers all-day comfort and endurance to get through the workday.",
                    PriceUSD = 32,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Jackets").Id,
                    Category =itemCategories.First(category=>category.Name =="Jackets")
                },
                new ShopItem()
                {
                    Name = "Women's Phoebe Maxi Dress",
                    Text = "Stylish and comfortable choice for any occasion.",
                    PriceUSD = 38,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Dresses").Id,
                    Category =itemCategories.First(category=>category.Name =="Dresses")
                },
                new ShopItem()
                {
                    Name = "Ladies Bohera Mandy Smocking Brown Dress",
                    Text = "Stylish and comfortable choice for any occasion.",
                    PriceUSD = 39,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Dresses").Id,
                    Category =itemCategories.First(category=>category.Name =="Dresses")
                },
                new ShopItem()
                {
                    Name = "Boho Tie Dye Brown Dress",
                    Text = "Beautiful, casual and functional, this dress is the perfect fit for Autumn!",
                    PriceUSD = 36,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Dresses").Id,
                    Category =itemCategories.First(category=>category.Name =="Dresses")
                },
                new ShopItem()
                {
                    Name = "Men's Woven Boxer Shorts",
                    Text = "Made with a blend of 65% cotton and 35% polyester, these men's boxers offer a soft, breathable feel with great durability.",
                    PriceUSD = 3,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Underwear").Id,
                    Category =itemCategories.First(category=>category.Name =="Underwear")
                },
                new ShopItem()
                {
                    Name = "Men's White Briefs",
                    Text = "Soft and breathable full rise cotton briefs are softer than ever and wick away moisture.",
                    PriceUSD = 3,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Underwear").Id,
                    Category =itemCategories.First(category=>category.Name =="Underwear")
                },
                new ShopItem()
                {
                    Name = "Men's Stretch Cotton Boxer Briefs",
                    Text = "Crafted in a premium soft, breathable cotton blended with stretch for the perfect snug fit.",
                    PriceUSD = 3,
                    Quantity = 100,
                    CategoryId = itemCategories.First(category=>category.Name =="Underwear").Id,
                    Category =itemCategories.First(category=>category.Name =="Underwear")
                },

            };
        }
        private IEnumerable<OrderStatus> GetOrderStatuses()
        {
            return new List<OrderStatus>()
            {
                new OrderStatus()
                {
                    Name = "AwaitingPayment",
                },
                new OrderStatus()
                {
                    Name = "Payed",
                },
                new OrderStatus()
                {
                    Name = "Processing",
                },
                new OrderStatus()
                {
                    Name = "Shipped",
                },
                new OrderStatus()
                {
                    Name = "Complete",
                },
                new OrderStatus()
                {
                    Name = "Canceled",
                },
            };
        }
        private IEnumerable<User> GetUsers()
        {
            var adminUser = new User()
            {
                UserName = "Admin",
                IsAdmin = true,
                Email = "admin@mail.com",
                Cart = new ShoppingCart(),
            };
            adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "qwerty");
            adminUser.Cart.User = adminUser;

            var regularUser = new User()
            {
                UserName = "User",
                IsAdmin = false,
                Email = "user@mail.com",
                Cart = new ShoppingCart(),
            };
            regularUser.PasswordHash = _passwordHasher.HashPassword(regularUser, "qwerty");
            regularUser.Cart.User = regularUser;

            return new List<User>() { adminUser, regularUser };
        }
    }
}
