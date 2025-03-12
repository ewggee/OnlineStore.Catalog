using AutoMapper;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Domain.Entities;

namespace OnlineStore.Catalog.Infrastructure.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<ShortProductDto, Product>()
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.MainImageUrl))
            .ReverseMap();
    }
}
