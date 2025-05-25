using AutoMapper;
using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.Mappings;

internal class RoutePlanMap : Profile
{
    public RoutePlanMap()
    {
        CreateMap<RoutePlan, RoutePlan>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore())
            .ForMember(dest => dest.Driver, opt => opt.Ignore())
            .ForMember(dest => dest.Tractor, opt => opt.Ignore());
    }
}
