using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
    public interface IShoppingCartServices
    {
        Task AddItemAsync(ShoppingCartItemDto shoppingCartItemDto);
        Task RemoveItemAsync(ShoppingCartItemDto shoppingCartItemDto);
    }
    public class ShoppingCartServices : IShoppingCartServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IUserServices _userServices;
        private readonly IUserContextService _userContextService;
        public ShoppingCartServices(MyShopDbContext dbContext, IUserServices userServices, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _userServices = userServices;
            _userContextService = userContextService;
        }

     
        public async Task AddItemAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);
            for (int i = 0; i < shoppingCartItemDto.Quantity; i++) user.Cart.ShopItemsIds.Add(shoppingCartItemDto.ItemId);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);
            for (int i = 0; i < shoppingCartItemDto.Quantity; i++) user.Cart.ShopItemsIds.Remove(shoppingCartItemDto.ItemId);
            await _dbContext.SaveChangesAsync();
        }   
   
    }
}
