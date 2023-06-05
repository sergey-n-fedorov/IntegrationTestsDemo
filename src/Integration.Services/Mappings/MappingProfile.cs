using AutoMapper;
using Integration.Data.Entities;
using Integration.Services.External;
using Integration.Shared.Models;

namespace Integration.Services.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserEntity, User>();
        CreateMap<ExternalUser, UserEntity>()
            .ForMember(x => x.Id,  _ => _.Ignore())
            .ForMember(x => x.ExternalId, _ => _.MapFrom(x => x.Id));
    }
}