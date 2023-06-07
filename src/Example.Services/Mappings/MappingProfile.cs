using AutoMapper;
using Example.Services.External;
using Example.Data.Entities;
using Example.Shared.Models;

namespace Example.Services.Mappings;

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