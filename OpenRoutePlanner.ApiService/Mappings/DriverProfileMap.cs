using AutoMapper;
using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.Mappings;

internal class DriverProfileMap : Profile
{
    public DriverProfileMap()
    {
        CreateMap<DriverProfile, DriverProfile>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore());
    }
}
