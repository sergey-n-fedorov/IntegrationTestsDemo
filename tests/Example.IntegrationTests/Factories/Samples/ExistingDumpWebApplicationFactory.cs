using Example.IntegrationTests.TestContainers;
using JetBrains.Annotations;
using Testcontainers.PostgreSql;

namespace Example.IntegrationTests.Factories.Samples;

[UsedImplicitly]
public class ExistingDumpWebApplicationFactory: BaseWebApplicationFactory
{
    protected override PostgreSqlContainer BuildPostgreSqlContainer() => 
        new PostgreSqlBuilder()
            .WithPortBinding(51111, PostgreSqlBuilder.PostgreSqlPort)
            .WithDatabase(DatabaseName)
            .WithDatabaseBackupMapping(TestDataProvider.DatabaseDumpPath)
            .Build();
    
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await PostgresContainer.RestoreDatabaseAsync(DatabaseName);;
    }
}