using Integration.IntegrationTests.Factories;

namespace Integration.IntegrationTests.Tests.Basic;

public abstract class ClassFixtureTests : IClassFixture<BaseWebApplicationFactory>
{
    private readonly BaseWebApplicationFactory _factory;

    public ClassFixtureTests(BaseWebApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Test(int index)
    {
        //Act
        var result = await _factory.CreateClient().PostAsync("api/v1/User/fetch", new StringContent(""));
        await Task.Delay(TimeSpan.FromSeconds(3));
        
        //Assert
        result.EnsureSuccessStatusCode();
    }
}

public class ClassFixture1Tests : ClassFixtureTests
{
    public ClassFixture1Tests(BaseWebApplicationFactory factory) : base(factory)
    {
    }
}

public class ClassFixture2Tests : ClassFixtureTests
{
    public ClassFixture2Tests(BaseWebApplicationFactory factory) : base(factory)
    {
    }
}

public class ClassFixture3Tests : ClassFixtureTests
{
    public ClassFixture3Tests(BaseWebApplicationFactory factory) : base(factory)
    {
    }
}