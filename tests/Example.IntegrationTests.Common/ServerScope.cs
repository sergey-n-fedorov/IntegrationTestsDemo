using Example.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Example.IntegrationTests;


//For context and scoped services
public class ServerScope : IDisposable
{
    public IServiceProvider ServiceProvider => _scope.ServiceProvider;

    private readonly IServiceScope _scope;

    public ServerScope(IServiceProvider factoryServiceProvider, ExampleContext? exampleContext)
    {
        _scope = factoryServiceProvider.CreateScope();

        var contextProvider = _scope.ServiceProvider.GetRequiredService<IExampleContextProvider>();
        var existingContext = contextProvider.Get();
        
        if (existingContext != null) {
            _scope.Dispose();
            throw new InvalidOperationException("Error creating WebApplicationFactory Server Scope, ExampleContext is already set");
        }

        var contextWriter = _scope.ServiceProvider.GetRequiredService<IExampleContextProvider>();
        contextWriter.Set(exampleContext);
    }

    public void Dispose()
    {
        var contextWriter = _scope.ServiceProvider.GetRequiredService<IExampleContextProvider>();
        contextWriter.Set(null);
        _scope.Dispose();
    }
}
