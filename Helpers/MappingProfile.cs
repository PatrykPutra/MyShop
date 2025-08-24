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
            CreateMap<Order, OrderDto>();
            CreateMap<ShopItem, ShopItemDto>()
                .ForMember(shopItemDto=> shopItemDto.Price, config => config.MapFrom(shopItem=>shopItem.PriceUSD))
                .ForMember(shopItemDto => shopItemDto.PriceCurrency, config => config.MapFrom(currencyName=>"USD"));
            CreateMap<CreateShopItemDto, ShopItem>();
        }
    }
}
