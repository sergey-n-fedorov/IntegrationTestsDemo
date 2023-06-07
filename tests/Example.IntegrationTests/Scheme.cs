using Example.Api.Controllers;
using Example.IntegrationTests.Factories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Example.IntegrationTests;

public class ComplexWebApplicationFactory : WebApplicationFactory<UserController>, IAsyncLifetime
{
    //Create Containers
    
    //Create Mock Setup
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //Override registrations with mock setup
           
            //Replace application settings with those of a test container
        });
        
        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        //Start Containers
        
        //Seed+create dump files OR restore from dump files
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        //Stop Containers
    }
}

public abstract class BaseEndpointTests : IClassFixture<ExampleWebApplicationFactory>, IDisposable
{
   
    protected BaseEndpointTests(ExampleWebApplicationFactory factory)
    {
        //Reset and Setup Mocks
        
        //Create UserContext scope
    }

    [Fact] //Mark test context as changing state if needed
    public void Test()
    {
        //Override Mock Setups if needed
    }

    public void Dispose()
    {
        //Dispose UserContext scope
        
        //If Marked as changing state Restore TestContainers state from dump 
    }
}