using Integration.IntegrationTests.Factories;
using Integration.IntegrationTests.Tools;
using Integration.Services;
using Integration.Shared;
using Integration.Shared.Clients;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Integration.IntegrationTests.Tests;

public class UserEndpointTests : IClassFixture<IntegrationWebApplicationFactory>, IDisposable
{
    private readonly IDisposable _contextScope;
    private readonly IntegrationWebApplicationFactory _factory;
    private readonly ITestOutputHelper _output;
    
    private static readonly Guid TestCorrelationId = new("11111111-1111-1111-1111-111111111111"); 
    
    protected WebApplicationFactoryScope CreateServerScope(Guid correlationId) =>  new(_factory.Services, new ExampleContext(correlationId));
    protected IIntegrationServiceClient GetClient() => _factory.ClientServiceProvider.GetClient();
    protected IDisposable CreateClientScope(ExampleContext exampleContext) => _factory.ClientServiceProvider.CreateExampleContextScope(exampleContext);

    public UserEndpointTests(IntegrationWebApplicationFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;

        _contextScope = CreateClientScope(new ExampleContext(TestCorrelationId));
    }

    [Fact]
    public async Task Test()
    {
        await GetClient().FetchUsersAsync();
        _output.WriteLine("Done");
    }

    [Fact]
    public async Task Test_ClientScope()
    {
        using (CreateClientScope(new ExampleContext(Guid.NewGuid())))
        {
            await GetClient().FetchUsersAsync();
        }
    }
    
    [Fact]
    public async Task Test_ServerScope()
    {
        using (var scope = CreateServerScope(Guid.NewGuid()))
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            await userService.FetchAsync();
        }
    }

    public void Dispose()
    {
        _contextScope.Dispose();
    }
}