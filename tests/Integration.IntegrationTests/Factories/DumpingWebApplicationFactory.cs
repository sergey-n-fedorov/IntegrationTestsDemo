using Integration.IntegrationTests.Tools.TestContainers;
using Integration.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.IntegrationTests.Factories;

public class DumpingWebApplicationFactory: BaseWebApplicationFactory
{
    public  Task RestoreFromDumpAsync() => PostgresContainer.RestoreDatabaseAsync(DatabaseName);
    
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await SeedAsync();
    }
    
    private async Task SeedAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            await userService.FetchAsync();
        }
       
        await PostgresContainer.DumpDatabaseAsync(DatabaseName);
    }
}