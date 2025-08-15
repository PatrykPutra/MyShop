using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
    public interface IShoppingCartServices
    {
        Task AddItemAsync(ShoppingCartItemDto shoppingCartItemDto, int userId);
        Task RemoveItemAsync(ShoppingCartItemDto shoppingCartItemDto, int userId);
    }
    public class ShoppingCartServices : IShoppingCartServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IUserServices _userServices;
        public ShoppingCartServices(MyShopDbContext dbContext, IUserServices userServices)
        {
            _dbContext = dbContext;
            _userServices = userServices;
        }

     
        public async Task AddItemAsync(ShoppingCartItemDto shoppingCartItemDto,int userId)
        {
            User user = await _userServices.GetAsync(userId);
            for (int i = 0; i < shoppingCartItemDto.Quantity; i++) user.Cart.ShopItemsIds.Add(shoppingCartItemDto.ItemId);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(ShoppingCartItemDto shoppingCartItemDto,int userId)
        {

            User user = await _userServices.GetAsync(userId);
            for (int i = 0; i < shoppingCartItemDto.Quantity; i++) user.Cart.ShopItemsIds.Remove(shoppingCartItemDto.ItemId);
            await _dbContext.SaveChangesAsync();
        }   
           

       
    }
}
