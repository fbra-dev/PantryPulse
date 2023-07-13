using AutoMapper;
using Backend.WebApi.Controllers.Models;
using Backend.WebApi.Domain.Models;

namespace Backend.WebApi.MappingProfiles;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        this.CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.ID,
                options => options.MapFrom(src => src.ID))
            .ForMember(
                dest => dest.Name,
                options => options.MapFrom(src => src.Name))
            .ForMember(
                dest => dest.RegularWeight,
                options => options.MapFrom(src => src.RegularWeight))
            .ForMember(
                dest => dest.AutomatedActions,
                options => options.MapFrom(src => src.AutomatedActions));
        
        this.CreateMap<ProductDto, Product>()
            .ForMember(
                dest => dest.ID,
                options => options.MapFrom(src => src.ID))
            .ForMember(
                dest => dest.Name,
                options => options.MapFrom(src => src.Name))
            .ForMember(
                dest => dest.RegularWeight,
                options => options.MapFrom(src => src.RegularWeight))
            .ForMember(
                dest => dest.AutomatedActions,
                options => options.MapFrom(src => src.AutomatedActions));
    }
}