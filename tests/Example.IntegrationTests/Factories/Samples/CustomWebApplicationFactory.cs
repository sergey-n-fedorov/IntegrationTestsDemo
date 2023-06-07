using Example.Api.Controllers;
using Example.Data;
using Example.IntegrationTests.TestContainers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Example.IntegrationTests.Factories.Samples;

public class CustomWebApplicationFactory : WebApplicationFactory<UserController>, IAsyncLifetime
{
    //create container
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder().Build();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //replace application settings with those of a test container
            services.AddSingleton(new IntegrationContextConfiguration
            {
                ConnectionString = _postgresContainer.GetConnectionString()
            });
        });

        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        //start container
        await _postgresContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        //dispose container
        await _postgresContainer.DisposeAsync();
    }
}