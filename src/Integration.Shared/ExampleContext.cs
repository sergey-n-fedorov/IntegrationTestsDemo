namespace Integration.Shared;

public class ExampleContext
{
    public ExampleContext(Guid? correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid? CorrelationId { get; }
}

public interface IExampleContextProvider
{
    ExampleContext? Get();

    void Set(ExampleContext? context);
}

public class ExampleContextProvider : IExampleContextProvider
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
}