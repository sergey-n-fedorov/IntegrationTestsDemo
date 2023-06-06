using Integration.Api.Controllers;
using Integration.Data;
using Integration.IntegrationTests.Tools.TestContainers;
using Integration.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Integration.IntegrationTests.Factories;

public class DumpWebApplicationFactory: WebApplicationFactory<UserController>, IAsyncLifetime
{
    private const string DatabaseName = "integration_tests";
    
    private readonly PostgreSqlContainer _postgresSqlContainer =
        new PostgreSqlBuilder()
            .WithDatabase(DatabaseName)
            .Build();
    
    public  Task RestoreFromDumpAsync() => _postgresSqlContainer.RestoreDatabaseAsync(DatabaseName);
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(new MockSetup().ExternalServiceClient.Object);
            services.AddSingleton(new IntegrationContextConfiguration { ConnectionString = _postgresSqlContainer.GetConnectionString() + ";Pooling=false" });
        });
        
        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _postgresSqlContainer.StartAsync();
        await SeedAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgresSqlContainer.DisposeAsync();
    }

    private async Task SeedAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            await userService.FetchAsync();
        }
       
        await _postgresSqlContainer.DumpDatabaseAsync(DatabaseName);
    }
}