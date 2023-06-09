using Example.Shared;
using Example.Shared.Context;

namespace Example.IntegrationTests;

public class TestExampleContextProvider : IExampleContextProvider
{
    private static readonly AsyncLocal<ExampleContext?> AsyncLocal = new();
    
    public ExampleContext? Get()
    {
        return AsyncLocal.Value;
    }

    public void Set(ExampleContext? context)
    {
        AsyncLocal.Value = context;
    } 
    
    public IDisposable CreateScope(ExampleContext? userContext)
    {
        ExampleContext? userContextBackup = AsyncLocal.Value;

        Set(userContext);

        return new DisposableAction(() => Set(userContextBackup));
    }
}