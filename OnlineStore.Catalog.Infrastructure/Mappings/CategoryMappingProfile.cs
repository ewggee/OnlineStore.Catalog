using AutoMapper;
using OnlineStore.Catalog.Contracts.Dtos;
using OnlineStore.Catalog.Domain.Entities;

namespace OnlineStore.Catalog.Infrastructure.Mappings;

public sealed class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(
                dest => dest.Id, 
                opt => opt.MapFrom(src => src.Id))
            .ReverseMap();
    }
}
