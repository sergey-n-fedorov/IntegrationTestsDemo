using Npgsql;

namespace Example.IntegrationTests.Extensions;

public static class StringExtensions
{
    public static string WithPoolingDisabled(this string postgresConnectionString)
    {
        NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(postgresConnectionString)
        {
            Pooling = false
        };

        return builder.ToString();
    }
}