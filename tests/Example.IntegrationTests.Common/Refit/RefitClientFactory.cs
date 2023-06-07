using Example.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Example.IntegrationTests.Refit;

public class RefitClientFactory<TServiceClient, TEntryPoint> where TEntryPoint : class where TServiceClient : class
{
    public TServiceClient CreateClient() => _serviceProvider.Value.GetRequiredService<TServiceClient>();
    
    public IDisposable CreateExampleContextScope(ExampleContext? exampleContext) => _serviceProvider.Value.GetRequiredService<TestExampleContextProvider>().CreateScope(exampleContext);
    
    private readonly Lazy<IServiceProvider> _serviceProvider;

    public RefitClientFactory(WebApplicationFactory<TEntryPoint> webApplicationFactory)
    {
        _serviceProvider = new Lazy<IServiceProvider>(BuildServiceProvider(webApplicationFactory));
    }

    private IServiceProvider BuildServiceProvider(WebApplicationFactory<TEntryPoint> webApplicationFactory)
    {
        var serviceCollection = new ServiceCollection();
        
        //RefitClient
        serviceCollection.AddSingleton(webApplicationFactory);
        serviceCollection.AddRefitClient<TServiceClient>();
        serviceCollection.RegisterCustomHttpClientFactoryForRefit<TEntryPoint>();
        
        //ExampleContext
        serviceCollection.AddSingleton(new TestExampleContextProvider());
        serviceCollection.AddSingleton<IExampleContextProvider>(w=>w.GetRequiredService<TestExampleContextProvider>());
        
        return serviceCollection.BuildServiceProvider();
    }
}