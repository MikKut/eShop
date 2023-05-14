using AutoMapper;
using Basket.Host.Models.Dtos;
using Basket.Host.Models.Responses;

namespace Basket.Host.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            _=CreateMap<OrderDto<CatalogItemDto>, GetBasketResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Orders));
        }
    }
}
