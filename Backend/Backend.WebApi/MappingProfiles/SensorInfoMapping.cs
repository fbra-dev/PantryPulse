using AutoMapper;
using Backend.WebApi.Controllers.Models;
using Backend.WebApi.Domain.Models;

namespace Backend.WebApi.MappingProfiles;

public class SensorInfoMapping : Profile
{
    public SensorInfoMapping()
    {
        this.CreateMap<SensorInfo, SensorInfoDto>()
            .ForMember(
                dest => dest.ID,
                options => options.MapFrom(src => src.ID))
            .ForMember(
                dest => dest.Name,
                options => options.MapFrom(src => src.Name))
            .ForMember(
                dest => dest.Weight,
                options => options.MapFrom(src => src.Weight));
    }
}