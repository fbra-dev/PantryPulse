using AutoMapper;
using Backend.WebApi.Controllers.Models;
using Backend.WebApi.Domain.Models;

namespace Backend.WebApi.MappingProfiles;

public class SensorMapping : Profile
{
    public SensorMapping()
    {
        this.CreateMap<Sensor, SensorDto>()
            .ForMember(
                dest => dest.ID,
                options => options.MapFrom(src => src.ID))
            .ForMember(
                dest => dest.Name,
                options => options.MapFrom(src => src.Name))
            .ForMember(
                dest => dest.Product,
                options => options.MapFrom(src => src.Product));

        this.CreateMap<SensorDto, Sensor>()
            .ForMember(
                dest => dest.ID,
                options => options.MapFrom(src => src.ID))
            .ForMember(
                dest => dest.Name,
                options => options.MapFrom(src => src.Name))
            .ForMember(
                dest => dest.Product,
                options => options.MapFrom(src => src.Product));
    }
}