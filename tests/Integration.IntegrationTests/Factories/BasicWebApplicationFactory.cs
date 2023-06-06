using Integration.Api.Controllers;
using Integration.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Integration.IntegrationTests.Factories;

public class BasicWebApplicationFactory: WebApplicationFactory<UserController>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresSqlContainer = new PostgreSqlBuilder().Build();
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(new MockSetup().ExternalServiceClient.Object);
            services.AddSingleton(new IntegrationContextConfiguration { ConnectionString = _postgresSqlContainer.GetConnectionString() });
        });
        
        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _postgresSqlContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgresSqlContainer.DisposeAsync();
    }
}