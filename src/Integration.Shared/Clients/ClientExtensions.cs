using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Integration.Shared.Clients;

public static  class ClientExtensions
{
    public static IServiceCollection AddFleetRefitClient<TClient>(this IServiceCollection serviceCollection,
        Func<IServiceProvider, string> baseUrlProviderFunc, Action<IHttpClientBuilder>? configureBuilder = null) where TClient : class
    {
        serviceCollection.AddTransient<ApiKeyHandler>();
        
        var refitSettings = new RefitSettings { ContentSerializer = new NewtonsoftJsonContentSerializer(HttpClientConsts.DefaultJsonSerializerSettings) };
        refitSettings.ExceptionFactory = new BaseResponseExceptionFactory(refitSettings.ExceptionFactory).ExecuteAsync;
        
        var builder = serviceCollection.AddRefitClient<TClient>(refitSettings)
            .ConfigureHttpClient((provider, client) => {
                    try {
                        client.BaseAddress = new Uri(baseUrlProviderFunc(provider));
                    }
                    catch (Exception e) {
                        throw new ArgumentException($"Cannot create {typeof(TClient)}, invalid Url setting", e);
                    }
                }
            )
            .AddHttpMessageHandler<ApiKeyHandler>();

        configureBuilder?.Invoke(builder);

        return serviceCollection;
    }
}