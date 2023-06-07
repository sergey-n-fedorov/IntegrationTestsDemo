using Example.IntegrationTests.Factories;

namespace Example.IntegrationTests.Tests.Basic;

[CollectionDefinition(nameof(CollectionFixture))]
public class CollectionFixture : ICollectionFixture<BaseWebApplicationFactory>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

[Collection(nameof(CollectionFixture))]
public abstract class CollectionFixtureTests 
{
    private readonly BaseWebApplicationFactory _factory;

    public CollectionFixtureTests(BaseWebApplicationFactory factory)
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

public class CollectionFixture1Tests : CollectionFixtureTests
{
    public CollectionFixture1Tests(BaseWebApplicationFactory factory) : base(factory)
    {
    }
}

public class CollectionFixture2Tests : CollectionFixtureTests
{
    public CollectionFixture2Tests(BaseWebApplicationFactory factory) : base(factory)
    {
    }
}