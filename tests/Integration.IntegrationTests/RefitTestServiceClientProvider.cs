using Integration.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Integration.IntegrationTests;

public class RefitTestServiceClientProvider<TServiceClient, TEntryPoint> where TEntryPoint : class where TServiceClient : class
{
    public TServiceClient GetClient() => _serviceProvider.Value.GetRequiredService<TServiceClient>();
    
    public IDisposable CreateExampleContextScope(ExampleContext? exampleContext) => _serviceProvider.Value.GetRequiredService<TestExampleContextProvider>().CreateScope(exampleContext);
    
    private readonly Lazy<IServiceProvider> _serviceProvider;

    public RefitTestServiceClientProvider(WebApplicationFactory<TEntryPoint> webApplicationFactory)
    {
        _serviceProvider = new Lazy<IServiceProvider>(BuildServiceProvider(webApplicationFactory));
    }

    private IServiceProvider BuildServiceProvider(WebApplicationFactory<TEntryPoint> webApplicationFactory)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(webApplicationFactory);
        serviceCollection.AddRefitClient<TServiceClient>();
        serviceCollection.RegisterCustomHttpClientFactoryForRefit<TEntryPoint>();
        // serviceCollection.AddSingleton(webApplicationFactory.Services.GetRequiredService<InternalApiOptions>());
        serviceCollection.AddSingleton(new TestExampleContextProvider());
        serviceCollection.AddSingleton<IExampleContextProvider>(w=>w.GetRequiredService<TestExampleContextProvider>());
        return serviceCollection.BuildServiceProvider();
    }
}