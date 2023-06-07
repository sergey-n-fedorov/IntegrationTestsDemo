using Example.IntegrationTests.Tools.TestContainers;
using Example.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Example.IntegrationTests.Factories;

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