using Example.Shared;
using Example.Shared.Context;

namespace Example.Api.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IExampleContextProvider _exampleContextProvider;

    public CorrelationIdMiddleware(IExampleContextProvider exampleContextProvider, RequestDelegate next)
    {
        _exampleContextProvider = exampleContextProvider;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(Constants.CorrelationIdHeaderName, out var correlationId) && !string.IsNullOrEmpty(correlationId))
        {
            _exampleContextProvider.Set(new ExampleContext(Guid.Parse(correlationId!)));
        }

        await _next(context);
    }
}