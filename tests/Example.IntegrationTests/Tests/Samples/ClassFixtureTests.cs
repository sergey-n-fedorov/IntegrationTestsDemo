using Example.IntegrationTests.Factories;

namespace Example.IntegrationTests.Tests.Samples;

public class ClassFixtureTests : IClassFixture<BaseWebApplicationFactory>
{
    private readonly BaseWebApplicationFactory _factory;

    public ClassFixtureTests(BaseWebApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Test(int index)
    {
        //Act
        var result = await _factory.CreateClient().PostAsync("api/v1/User/fetch", new StringContent(""));
        await Task.Delay(TimeSpan.FromSeconds(3));
        
        //Assert
        result.EnsureSuccessStatusCode();
    }
}

public class ClassFixtureTestsClone1 : ClassFixtureTests
{
    public ClassFixtureTestsClone1(BaseWebApplicationFactory factory) : base(factory)
    {
    }
}

public class ClassFixtureTestsClone2 : ClassFixtureTests
{
    public ClassFixtureTestsClone2(BaseWebApplicationFactory factory) : base(factory)
    {
    }
}

