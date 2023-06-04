using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Refit;

namespace Integration.IntegrationTests;

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
        serviceCollection.TryAddTransient<ApiKeyHandler>();
        serviceCollection.TryAddTransient<OperationContextPropagationHandler<UserContext>>();
        serviceCollection.RegisterCustomHttpClientFactory<TEntryPoint>(w => new DelegatingHandler[] {
            w.GetRequiredService<ApiKeyHandler>(),
            w.GetRequiredService<OperationContextPropagationHandler<UserContext>>()
        });
    }
    
    public static void AddTestRefitClient<TClient>(this IServiceCollection serviceCollection) where TClient : class
    {
        var refitSettings = new RefitSettings { ContentSerializer = new NewtonsoftJsonContentSerializer(HttpClientConsts.DefaultJsonSerializerSettings) };
        refitSettings.ExceptionFactory = new BaseResponseExceptionFactory(refitSettings.ExceptionFactory).ExecuteAsync;
        serviceCollection.AddRefitClient<TClient>(refitSettings);
    }
}