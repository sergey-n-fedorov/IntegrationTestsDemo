using StackExchange.Redis;
using Testcontainers.Redis;

namespace Example.IntegrationTests.Tests.Samples;

public class RedisTests
{
    [Fact]
    public async Task RedisTest()
    {
        var redisContainer = new RedisBuilder().Build();

        await redisContainer.StartAsync();

        await using (var connection = await ConnectionMultiplexer.ConnectAsync(redisContainer.GetConnectionString()))
        {
            connection.IsConnected.Should().BeTrue();
        }
        
        await redisContainer.ExecAsync(new List<string> { "ls" });

        await redisContainer.DisposeAsync();
    }
}