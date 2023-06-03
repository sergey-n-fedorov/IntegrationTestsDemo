using Integration.Data.Repositories;
using Integration.Services.Mappings;
using IntegrationService.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Services.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddDbContext<IntegrationContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddAutoMapper(typeof(MappingProfile));
        
        return services;
    }
}