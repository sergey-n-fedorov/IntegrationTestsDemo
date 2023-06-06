using Integration.Api.Controllers;
using Integration.Data;
using Integration.IntegrationTests.Tools.Refit;
using Integration.Shared.Clients;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Integration.IntegrationTests.Factories;

public class IntegrationWebApplicationFactory : WebApplicationFactory<UserController>, IAsyncLifetime
{
    private const string DatabaseName = "integration_tests";
    
    public ClientServiceProvider<IIntegrationServiceClient, UserController> ClientServiceProvider = null!;
    
    public MockSetup MockSetup { get; } = new();
    
    private readonly PostgreSqlContainer _postgresSqlContainer = new PostgreSqlBuilder()
        .WithPortBinding(51111, PostgreSqlBuilder.PostgreSqlPort)
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
        
        ClientServiceProvider = new ClientServiceProvider<IIntegrationServiceClient, UserController>(this);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgresSqlContainer.DisposeAsync();
    }
}