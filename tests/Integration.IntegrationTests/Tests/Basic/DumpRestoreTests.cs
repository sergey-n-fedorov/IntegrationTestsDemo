using Integration.IntegrationTests.Factories;
using Integration.Shared.Models;
using Newtonsoft.Json;

namespace Integration.IntegrationTests.Tests.Basic;

public class DumpRestoreTests : IClassFixture<DumpingWebApplicationFactory>, IDisposable
{
    private readonly DumpingWebApplicationFactory _factory;

    public DumpRestoreTests(DumpingWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Theory]
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
        _factory.RestoreFromDumpAsync().Wait();
    }
}