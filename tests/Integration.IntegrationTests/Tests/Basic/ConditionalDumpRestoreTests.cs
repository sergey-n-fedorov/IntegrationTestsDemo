using Integration.IntegrationTests;
using Integration.IntegrationTests.Factories;
using Integration.IntegrationTests.Tools;
using Integration.Shared.Models;
using Newtonsoft.Json;

public class ConditionalDumpRestoreTests : IClassFixture<DumpWebApplicationFactory>, IDisposable
{
    private readonly DumpWebApplicationFactory _factory;

    public ConditionalDumpRestoreTests(DumpWebApplicationFactory factory)
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


    [Theory, ChangesState]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
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
        if (IntegrationTestContext.Current.StateChanged)
        {
            _factory.RestoreFromDumpAsync().Wait();
        }
    }
}