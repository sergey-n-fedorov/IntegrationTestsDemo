using Example.IntegrationTests.Factories;
using Example.Services;
using Example.Shared;
using Example.Shared.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Example.IntegrationTests.Tests;

public class BaseEndpointTests : IClassFixture<ExampleWebApplicationFactory>, IDisposable
{
    private readonly IDisposable _contextScope;
    private readonly ExampleWebApplicationFactory _factory;
    
    protected static readonly Guid TestCorrelationId = Guid.NewGuid(); 
    
    protected ServerScope CreateServerScope(Guid correlationId) =>  new(_factory.Services, new ExampleContext(correlationId));
    protected IDisposable CreateExampleContextScope(ExampleContext exampleContext) => _factory.ClientFactory.CreateExampleContextScope(exampleContext);
    protected IIntegrationServiceClient CreateClient() => _factory.ClientFactory.CreateClient();

    public BaseEndpointTests(ExampleWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.MockSetup.ResetAndSetup();
        _contextScope = CreateExampleContextScope(new ExampleContext(TestCorrelationId));
    }

    public void Dispose()
    {
        _contextScope.Dispose();
    }
}


public class UserEndpointTests : IClassFixture<ExampleWebApplicationFactory>, IDisposable
{
    private readonly IDisposable _contextScope;
    private readonly ExampleWebApplicationFactory _factory;
    
    public static readonly Guid TestCorrelationId = Guid.NewGuid(); 
    
    protected ServerScope CreateServerScope(Guid correlationId) =>  new(_factory.Services, new ExampleContext(correlationId));
    protected IDisposable CreateExampleContextScope(ExampleContext exampleContext) => _factory.ClientFactory.CreateExampleContextScope(exampleContext);
    protected IIntegrationServiceClient CreateClient() => _factory.ClientFactory.CreateClient();

    public UserEndpointTests(ExampleWebApplicationFactory factory)
    {
        _factory = factory;

        _factory.MockSetup.ResetAndSetup();
        _contextScope = CreateExampleContextScope(new ExampleContext(TestCorrelationId));
    }

    [Fact]
    public async Task Test()
    {
        //Act
        var users  = await CreateClient().GetUsersAsync();

        //Assert
        users.Count.Should().Be(TestDataProvider.ExternalUsers.Count);
    }

    [Fact]
    public async Task Test_ClientScope()
    {
        using (CreateExampleContextScope(new ExampleContext(Guid.NewGuid())))
        {
            var users  = await CreateClient().GetUsersAsync();
        }
    }
    
    [Fact]
    public async Task Test_ServerScope()
    {
        using (var scope = CreateServerScope(Guid.NewGuid()))
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var users  = await userService.GetAllAsync();
        }
    }
    
    [Fact]
    public async Task Test_MockSetupOverride()
    {
        //Arrange
        var users = await CreateClient().GetUsersAsync();
        var userToDeleteId = users.First().Id;

        _factory.MockSetup.ExternalServiceClient.Setup(w => w.DeleteUser(It.IsAny<string>())).ThrowsAsync(new Exception());

        //Act and Assert
        Func<Task> act = async () => { await CreateClient().DeleteUserAsync(userToDeleteId); };
        await act.Should().ThrowAsync<Exception>();
    }

    public void Dispose()
    {
        _contextScope.Dispose();
    }
}