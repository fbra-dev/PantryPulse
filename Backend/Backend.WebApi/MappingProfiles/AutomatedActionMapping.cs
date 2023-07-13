using AutoMapper;
using Backend.WebApi.Controllers.Models;
using Backend.WebApi.Domain.Models;

namespace Backend.WebApi.MappingProfiles;

public class AutomatedActionMapping : Profile
{
    public AutomatedActionMapping()
    {
        this.CreateMap<AutomatedAction, AutomatedActionDto>()
            .ForMember(
                dest => dest.ID,
                options => options.MapFrom(src => src.ID))
            .Include<CallWebServiceAction, CallWebServiceActionDto>()
            .Include<PushNotificationAction, PushNotificationActionDto>();
        this.CreateMap<CallWebServiceAction, CallWebServiceActionDto>()
            .ForMember(
                dest => dest.Url,
                options => options.MapFrom(src => src.Url))
            .ForMember(
                dest => dest.Parameters,
                options => options.MapFrom(src => src.Parameters));
        this.CreateMap<PushNotificationAction, PushNotificationActionDto>()
            .ForMember(
                dest => dest.CustomText,
                options => options.MapFrom(src => src.CustomText));
    }
}