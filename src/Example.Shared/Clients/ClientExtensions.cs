using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Example.Shared.Clients;

public static  class ClientExtensions
{
    public static IServiceCollection AddFleetRefitClient<TClient>(this IServiceCollection serviceCollection) where TClient : class
    {
        serviceCollection.AddTransient<CorrelationIdHandler>();

        serviceCollection.AddRefitClient<TClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.external.com"))
            .AddHttpMessageHandler<CorrelationIdHandler>();

        return serviceCollection;
    }
}