using Example.Api.Controllers;
using Example.Data;
using Example.IntegrationTests.TestContainers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Example.IntegrationTests.Factories.Samples;

public class BaseWebApplicationFactory: WebApplicationFactory<UserController>, IAsyncLifetime
{
    protected const string DatabaseName = "integration_tests";
    
    protected virtual PostgreSqlContainer BuildPostgreSqlContainer() => 
        new PostgreSqlBuilder()
        // .WithPortBinding(51111, PostgreSqlBuilder.PostgreSqlPort)
        .WithDatabase(DatabaseName)
        .Build();

    protected PostgreSqlContainer PostgresContainer { get; private set; } = null!;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(new MockSetup().ExternalServiceClient.Object);
            services.AddSingleton(new IntegrationContextConfiguration { ConnectionString = PostgresContainer.GetConnectionStringWithPoolingDisabled() });
        });
        
        return base.CreateHost(builder);
    }

    public virtual async Task InitializeAsync()
    {
        PostgresContainer = BuildPostgreSqlContainer();
        await PostgresContainer.StartAsync();
    }

    public new virtual async Task DisposeAsync()
    {
        await PostgresContainer.DisposeAsync();
    }
}