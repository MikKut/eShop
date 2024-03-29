﻿using AutoMapper;
using MVC.Models.Dto;
using MVC.Models.Domains;

namespace MVC.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            _=CreateMap<CatalogType, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Type));
            _=CreateMap<CatalogBrand, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Brand));
            _=CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => int.Parse(src.Id)))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.Join(' ', src.Name, src.LastName)));
            _=CreateMap<CatalogItem, CatalogItemDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.CatalogBrand.Brand))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.CatalogType.Type));
        }
    }
}
