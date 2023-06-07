using Example.IntegrationTests.Factories;
using Example.Services;
using Example.Shared;
using Example.Shared.Clients;
using Example.Shared.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Example.IntegrationTests.Tests;

public abstract class BaseEndpointTests : IClassFixture<ExampleWebApplicationFactory>, IDisposable
{
    protected readonly ExampleWebApplicationFactory Factory;

    private static readonly Guid TestCorrelationId = Guid.NewGuid(); 
    
    protected ServerScope CreateServerScope(Guid correlationId) =>  new(Factory.Services, new ExampleContext(correlationId));
    protected IDisposable CreateExampleContextScope(ExampleContext exampleContext) => Factory.ClientFactory.CreateExampleContextScope(exampleContext);
    protected IIntegrationServiceClient CreateClient() => Factory.ClientFactory.CreateClient();
    
    private readonly IDisposable _exampleContextScope;

    protected BaseEndpointTests(ExampleWebApplicationFactory factory)
    {
        Factory = factory;
        Factory.MockSetup.ResetAndSetup();
        _exampleContextScope = CreateExampleContextScope(new ExampleContext(TestCorrelationId));
    }

    public void Dispose()
    {
        _exampleContextScope.Dispose();
    }
}


public class UserEndpointTests : BaseEndpointTests
{
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
        List<User> users;
        
        using (var scope = CreateServerScope(Guid.NewGuid()))
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            users  = await userService.GetAllAsync();
           
        }
        
        users.Count.Should().Be(TestDataProvider.ExternalUsers.Count);
    }
    
    [Fact]
    public async Task Test_MockSetupOverride()
    {
        //Arrange
        var users = await CreateClient().GetUsersAsync();
        var userToDeleteId = users.First().Id;

        Factory.MockSetup.ExternalServiceClient.Setup(w => w.DeleteUser(It.IsAny<string>())).ThrowsAsync(new Exception());

        //Act and Assert
        Func<Task> act = async () => { await CreateClient().DeleteUserAsync(userToDeleteId); };
        await act.Should().ThrowAsync<Exception>();
    }


    public UserEndpointTests(ExampleWebApplicationFactory factory) : base(factory)
    {
    }
}