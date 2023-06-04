using AutoMapper;
using Integration.Data.Entities;
using Integration.Shared.Models;

namespace Integration.Services.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserEntity, User>();
    }
}