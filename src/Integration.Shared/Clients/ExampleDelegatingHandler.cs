namespace Integration.Shared.Clients;

public class ExampleDelegatingHandler: DelegatingHandler
{
    private readonly IExampleContextProvider _exampleContextProvider;

    public ExampleDelegatingHandler(IExampleContextProvider exampleContextProvider)
    {
        _exampleContextProvider = exampleContextProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _exampleContextProvider.Set(new ExampleContext { CorrelationId = Guid.NewGuid() });
        return await base.SendAsync(request, cancellationToken);
    }
}