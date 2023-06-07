using Npgsql;
using Testcontainers.PostgreSql;
using static Example.IntegrationTests.TestContainers.TestContainerTools;

namespace Example.IntegrationTests.TestContainers;

public static class PostgreSqlContainerExtensions 
{
    private const string  DumpFilePathInContainer = "backup.dump";
    
    public static async Task DumpDatabaseAsync(this PostgreSqlContainer container, string databaseName)
    {
        await container.ExecWithExceptionAsync(SplitCommand($"pg_dump -Fc -Z 9 -U postgres {databaseName} -f {DumpFilePathInContainer}"));
    }
    
    public static async Task RestoreDatabaseAsync(this PostgreSqlContainer container, string databaseName)
    {
        List<string> dropDbCommand = SplitCommand("psql -U postgres -d postgres -c");
        dropDbCommand.Add($"DROP DATABASE {databaseName} WITH (FORCE);");
        await container.ExecWithExceptionAsync(dropDbCommand);
        
        
        List<string> createDbCommand = SplitCommand("psql -U postgres -d postgres -c");
        createDbCommand.Add($"CREATE DATABASE {databaseName};");
        await container.ExecWithExceptionAsync(createDbCommand);
        
        await container.ExecWithExceptionAsync(SplitCommand($"pg_restore -Fc -j 8 -U postgres -d {databaseName} {DumpFilePathInContainer}"));
    }
    
    public static PostgreSqlBuilder WithDatabaseBackupMapping(this PostgreSqlBuilder builder, string dumpFileHostPath)
    {
        return builder.WithResourceMapping(dumpFileHostPath, $"/{DumpFilePathInContainer}");
    }
    
    public static string GetConnectionStringWithPoolingDisabled(this PostgreSqlContainer container)
    {
        NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(container.GetConnectionString())
        {
            Pooling = false
        };

        return builder.ToString();
    }
}