using AutoMapper;
using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.Mappings;

internal class TractorMap : Profile
{
    public TractorMap()
    {
        CreateMap<Tractor, Tractor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Driver, opt => opt.Ignore());
    }
}
