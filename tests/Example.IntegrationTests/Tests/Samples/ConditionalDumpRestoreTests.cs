using Example.IntegrationTests.Factories;
using Example.IntegrationTests.Factories.Samples;
using Example.IntegrationTests.TestContext;
using Example.Shared.Models;
using Newtonsoft.Json;

namespace Example.IntegrationTests.Tests.Samples;

public class ConditionalDumpRestoreTests : IClassFixture<DumpingWebApplicationFactory>, IDisposable
{
    private readonly DumpingWebApplicationFactory _factory;

    public ConditionalDumpRestoreTests(DumpingWebApplicationFactory factory)
    {
        _factory = factory;
        
        IntegrationTestContext.Current = new IntegrationTestContext();
    }

    [Fact]
    public async Task FetchTest()
    {
        //Act
        var users = await GetAllUsers();
        
        //Assert
        users.Count.Should().Be(TestDataProvider.ExternalUsers.Count);
    }


    [Theory, DirtyTest]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeleteTest(int index)
    {
        //Arrange
        var users = await GetAllUsers();
        users.Count.Should().Be(TestDataProvider.ExternalUsers.Count);
        var userToDeleteId = users.First().Id;
        
        //Act
        await _factory.CreateClient().DeleteAsync($"api/v1/User/{userToDeleteId}");

        //Assert
        users = await GetAllUsers();
        users.Count.Should().Be(TestDataProvider.ExternalUsers.Count - 1);
        users.Should().NotContain(w => w.Id == userToDeleteId);
    }
    
    private async Task<List<User>> GetAllUsers()
    {
        var response = await _factory.CreateClient().GetAsync("api/v1/User");
        string jsonResult = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<User>>(jsonResult)!;
    }

    public void Dispose()
    {
        if (IntegrationTestContext.Current?.StateChanged == true)
        {
            _factory.RestoreFromDumpAsync().Wait();
        }
    }
}