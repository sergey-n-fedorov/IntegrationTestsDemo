using AutoMapper;
using Integration.Data.Models;
using Integration.Shared.Models;

namespace Integration.Services.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserEntity, User>();
    }
}