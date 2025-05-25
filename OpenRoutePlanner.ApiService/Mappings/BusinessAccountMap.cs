using AutoMapper;
using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.Mappings;

internal class BusinessAccountMap : Profile
{
    public BusinessAccountMap()
    {
        CreateMap<BusinessAccount, BusinessAccount>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
