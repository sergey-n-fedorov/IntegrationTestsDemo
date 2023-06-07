namespace Example.Shared.Clients;

public class CorrelationIdHandler: DelegatingHandler
{
    private readonly IExampleContextProvider _exampleContextProvider;

    public CorrelationIdHandler(IExampleContextProvider exampleContextProvider)
    {
        _exampleContextProvider = exampleContextProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = _exampleContextProvider.Get();
        request.Headers.Add(Constants.CorrelationIdHeaderName, context?.CorrelationId?.ToString());
        return await base.SendAsync(request, cancellationToken);
    }
}