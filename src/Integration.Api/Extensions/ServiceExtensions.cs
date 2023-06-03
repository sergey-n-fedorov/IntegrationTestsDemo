using Integration.Data.Repositories;
using IntegrationService.Data;

namespace Integration.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services)
    {
        services.AddDbContext<IntegrationContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}