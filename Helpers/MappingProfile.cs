using AutoMapper;
using MyShop.Models;

namespace MyShop.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ItemCategory,ItemCategoryDto>();
            CreateMap<ItemCategoryDto, ItemCategory>();
        }
    }
}
