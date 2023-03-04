using AutoMapper;
using MVC.Models.Dto;
using MVC.ViewModels.Models;
using MVC.ViewModels.Models;

namespace MVC.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogType, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Type));
            CreateMap<CatalogBrand, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Brand));
            CreateMap<CatalogItem, CatalogBasketItem>();
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.Join(' ', src.Name, src.LastName)));
            CreateMap<CatalogItem, CatalogItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.CatalogBrand.Brand))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.Join(' ', src.Name, src.LastName)));
        }
    }
}
