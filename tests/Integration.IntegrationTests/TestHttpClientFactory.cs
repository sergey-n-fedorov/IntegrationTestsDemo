using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace Integration.IntegrationTests;

public class TestHttpClientFactory<TEntryPoint> : IHttpClientFactory where TEntryPoint : class
{
    private readonly WebApplicationFactory<TEntryPoint> _webApplicationFactory;
    private readonly IOptionsMonitor<HttpClientFactoryOptions> _optionsMonitor;
    private readonly DelegatingHandler[] _delegatingHandlers;


    public TestHttpClientFactory(WebApplicationFactory<TEntryPoint> webApplicationFactory,
        IOptionsMonitor<HttpClientFactoryOptions> optionsMonitor,
        DelegatingHandler[] delegatingHandlers)
    {
        _webApplicationFactory = webApplicationFactory;
        _optionsMonitor = optionsMonitor;
        _delegatingHandlers = delegatingHandlers;
    }

    public HttpClient CreateClient(string name)
    {
        var client = _webApplicationFactory.CreateDefaultClient(_delegatingHandlers);

        var options = _optionsMonitor.Get(name);

        foreach (var httpClientAction in options.HttpClientActions) {
            httpClientAction(client);
        }

        return client;
    }
}