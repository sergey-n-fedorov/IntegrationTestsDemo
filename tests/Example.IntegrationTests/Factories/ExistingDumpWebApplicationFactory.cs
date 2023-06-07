using Example.IntegrationTests.TestContainers;
using Testcontainers.PostgreSql;

namespace Example.IntegrationTests.Factories;

public class ExistingDumpWebApplicationFactory: BaseWebApplicationFactory
{
    protected override PostgreSqlContainer BuildPostgreSqlContainer() => 
        new PostgreSqlBuilder()
            .WithPortBinding(51111, PostgreSqlBuilder.PostgreSqlPort)
            .WithDatabase(DatabaseName)
            .WithDatabaseBackupMapping("TestData/Dumps/integration_backup.dump")
            .Build();
    
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await PostgresContainer.RestoreDatabaseAsync(DatabaseName);;
    }
}