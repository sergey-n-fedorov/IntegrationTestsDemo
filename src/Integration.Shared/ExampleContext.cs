namespace Integration.Shared;

public class ExampleContext
{
    public Guid CorrelationId { get; set; }
}

public interface IExampleContextProvider
{
    ExampleContext Get();

    void Set(ExampleContext context);
}

public class ExampleContextProvider : IExampleContextProvider
{
    private static readonly AsyncLocal<ExampleContext> AsyncLocal = new();
    
    public ExampleContext Get()
    {
        return AsyncLocal.Value!;
    }

    public void Set(ExampleContext context)
    {
        AsyncLocal.Value = context;
    }
}