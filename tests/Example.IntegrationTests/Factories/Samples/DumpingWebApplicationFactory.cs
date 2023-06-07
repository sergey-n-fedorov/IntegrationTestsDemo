using Example.IntegrationTests.TestContainers;
using Example.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Example.IntegrationTests.Factories.Samples;

[UsedImplicitly]
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