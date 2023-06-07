using Example.Api.Controllers;
using Example.Data;
using Example.IntegrationTests.Refit;
using Example.Shared.Clients;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Example.IntegrationTests.Factories;

public class IntegrationWebApplicationFactory : WebApplicationFactory<UserController>, IAsyncLifetime
{
    private const string DatabaseName = "integration_tests";
    
    public RefitClientScopeFactory<IIntegrationServiceClient, UserController> ClientScopeFactory = null!;
    
    public MockSetup MockSetup { get; } = new();
    
    private readonly PostgreSqlContainer _postgresSqlContainer = new PostgreSqlBuilder()
        // .WithPortBinding(51111, PostgreSqlBuilder.PostgreSqlPort)
        .WithDatabase(DatabaseName)
        // .WithDatabaseBackupMapping("TestData/Dumps/data_backup.dump")
        .Build();
    
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(MockSetup.ExternalServiceClient.Object);
            services.AddSingleton(new IntegrationContextConfiguration { ConnectionString = _postgresSqlContainer.GetConnectionString() });
        });
        
        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _postgresSqlContainer.StartAsync();
        
        ClientScopeFactory = new RefitClientScopeFactory<IIntegrationServiceClient, UserController>(this);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgresSqlContainer.DisposeAsync();
    }
}