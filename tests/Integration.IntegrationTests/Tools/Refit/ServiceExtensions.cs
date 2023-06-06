using Integration.Shared.Clients;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace Integration.IntegrationTests.Tools.Refit;

public static class ServiceExtensions
{
    public static void RegisterCustomHttpClientFactory<TEntryPoint>(this IServiceCollection serviceCollection,
        Func<IServiceProvider, DelegatingHandler[]> delegatingHandlerFactory) where TEntryPoint : class
    {
        serviceCollection.AddTransient<IHttpClientFactory>(
            serviceProvider => new TestHttpClientFactory<TEntryPoint>(
                serviceProvider.GetRequiredService<WebApplicationFactory<TEntryPoint>>(),
                serviceProvider.GetRequiredService<IOptionsMonitor<HttpClientFactoryOptions>>(),
                delegatingHandlerFactory(serviceProvider)));
    }

    public static void RegisterCustomHttpClientFactoryForRefit<TEntryPoint>(this IServiceCollection serviceCollection) where TEntryPoint : class
    {
        serviceCollection.TryAddTransient<CorrelationIdHandler>();
        serviceCollection.RegisterCustomHttpClientFactory<TEntryPoint>(w => new DelegatingHandler[] {
            w.GetRequiredService<CorrelationIdHandler>(),
        });
    }
}