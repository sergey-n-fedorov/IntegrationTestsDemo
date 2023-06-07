using Example.Api.Controllers;
using Example.Data;
using Example.IntegrationTests.Refit;
using Example.IntegrationTests.TestContainers;
using Example.Shared.Clients;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Example.IntegrationTests.Factories;

[UsedImplicitly]
public class ExampleWebApplicationFactory : WebApplicationFactory<UserController>, IAsyncLifetime
{
    private const string DatabaseName = "integration_tests";
    
    public RefitClientFactory<IIntegrationServiceClient, UserController> ClientFactory = null!;
    
    public MockSetup MockSetup { get; } = new();
    
    private readonly PostgreSqlContainer _postgresSqlContainer = new PostgreSqlBuilder()
        // .WithPortBinding(51111, PostgreSqlBuilder.PostgreSqlPort)
        .WithDatabase(DatabaseName)
        .WithDatabaseBackupMapping(TestDataProvider.DatabaseDumpPath)
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
        await _postgresSqlContainer.RestoreDatabaseAsync(DatabaseName);
        
        ClientFactory = new RefitClientFactory<IIntegrationServiceClient, UserController>(this);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgresSqlContainer.DisposeAsync();
    }
}