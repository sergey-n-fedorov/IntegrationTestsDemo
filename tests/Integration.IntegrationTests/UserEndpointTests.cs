using Xunit.Abstractions;

namespace Integration.IntegrationTests;

public class Tests : IClassFixture<IntegrationWebApplicationFactory>
{
    
    private readonly IntegrationWebApplicationFactory _factory;
    private readonly ITestOutputHelper _output;

    public Tests(IntegrationWebApplicationFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
    }

    [Fact]
    public async Task  Test()
    {

        var client = _factory.CreateClient();
        var result = await client.PutAsync("/api/v1/user/fetch", new StringContent(""));
        _output.WriteLine("Done");
    }


}