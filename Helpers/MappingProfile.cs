using AutoMapper;
using MyShop.Entities;
using MyShop.Models;

namespace MyShop.Helpers
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<ItemCategory,CreateItemCategoryDto>();
            CreateMap<CreateItemCategoryDto, ItemCategory>();
            CreateMap<ItemCategory, ItemCategoryDto>();
            CreateMap<ItemCategoryDto, ItemCategory>();
            CreateMap<Order, OrderDto>()
                .ForMember(orderDto => orderDto.TotalPrice, config => config.MapFrom(order => order.TotalPriceUSD));
            CreateMap<ShopItem, ShopItemDto>()
                .ForMember(shopItemDto=> shopItemDto.Price, config => config.MapFrom(shopItem=>shopItem.PriceUSD))
                .ForMember(shopItemDto => shopItemDto.PriceCurrency, config => config.MapFrom(currencyName=>"USD"));
            CreateMap<CreateShopItemDto, ShopItem>();
            CreateMap<ShoppingCartItemDto, ShoppingCartItem>();
            CreateMap<ShoppingCartItem, ShoppingCartItemDto>();
            CreateMap<OrderItem, OrderItemGetDto>()
                .ForMember(orderItemGetDto => orderItemGetDto.Price, config => config.MapFrom(orderItem => orderItem.PriceUSD))
                .ForMember(orderItemGetDto => orderItemGetDto.CurrencyName, config => config.MapFrom(currencyName => "USD"));
            CreateMap<User, UserInfoDto>();
        }
    }
}
