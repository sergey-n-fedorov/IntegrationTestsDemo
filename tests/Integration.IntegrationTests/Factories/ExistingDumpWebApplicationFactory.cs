using Integration.IntegrationTests.Tools.TestContainers;
using Testcontainers.PostgreSql;

namespace Integration.IntegrationTests.Factories;

public class ExistingDumpWebApplicationFactory: BaseWebApplicationFactory
{
    protected override PostgreSqlContainer BuildPostgreSqlContainer() => 
        new PostgreSqlBuilder()
            .WithDatabase(DatabaseName)
            .WithDatabaseBackupMapping("TestData/Dumps/integration_backup.dump")
            .Build();

    public  Task RestoreFromDumpAsync() => PostgresContainer.RestoreDatabaseAsync(DatabaseName);
    
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await RestoreFromDumpAsync();
    }
}