using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
    public interface IOrderServices
    {
        Task AddAsync();
        Task<List<OrderDto>> GetAsync();
    }
    public class OrderServices : IOrderServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IUserServices _userServices;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        public OrderServices(MyShopDbContext dbContext, IUserServices userServices,IUserContextService userContextService,IMapper mapper)
        {
            _dbContext = dbContext;
            _userServices = userServices;
            _userContextService = userContextService;
            _mapper = mapper;
        }
        
        public async Task AddAsync()
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);

            List<int> orderItemsIds = new List<int>(user.Cart.ShopItemsIds);
            if (orderItemsIds.Count == 0) throw new ArgumentException("Cart is empty");

            List<OrderItem> orderItems = new List<OrderItem>();
            decimal totalPriceUSD = 0;

            foreach (var itemId in orderItemsIds)
            {
                ShopItem? shopItem = await _dbContext.ShopItems.FindAsync(itemId);
                if (shopItem == null) throw new ArgumentException($"Shop item no. {itemId} does not exist");
                if (orderItems.Any(order => order.ShopItemId == itemId))
                {
                    var order = orderItems.First(order => order.ShopItemId == itemId);
                    order.Quantity++;
                }
                else
                {
                    OrderItem orderItem = new OrderItem
                    {
                        Name = shopItem.Name,
                        ShopItemId = shopItem.Id,
                        PriceUSD = shopItem.PriceUSD,
                        Quantity = 1
                    };
                    orderItems.Add(orderItem);
                }
                totalPriceUSD += shopItem.PriceUSD;
                shopItem.Quantity--; // move to ShopItemServices
                if (shopItem.Quantity <= 0) throw new ArgumentException($"Not enough {shopItem.Name} in shop storage.");
            }
            
            Order newOrder = new Order
            {
                CreationDate = DateTime.Now,
                Items = orderItems,
                TotalPriceUSD = totalPriceUSD,
                User = user,
                UserId= user.Id,
                StatusId=1,
            };

            _dbContext.Orders.Add(newOrder);
            user.Cart.ShopItemsIds.Clear();
            await _dbContext.SaveChangesAsync();
            
        }
        public async Task<List<OrderDto>> GetAsync()
        {
            int userId = _userContextService.GetUserId();
            var orders = await _dbContext.Orders.Include(order=>order.Items).Where(order=>order.UserId == userId).ToListAsync();
            return _mapper.Map<List<OrderDto>>(orders);
        }
        // getAll
       
    }
}
