using Integration.IntegrationTests.Factories;
using Integration.Shared.Models;
using Newtonsoft.Json;

namespace Integration.IntegrationTests.Tests.Basic;

public class ExistingDumpTests : IClassFixture<ExistingDumpWebApplicationFactory>
{
    private readonly ExistingDumpWebApplicationFactory _factory;

    public ExistingDumpTests(ExistingDumpWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task FetchTest()
    {
        //Act
        var users = await GetAllUsers();

        //Assert
        users.Count.Should().Be(TestDataProvider.ExternalUsers.Count);
    }

    private async Task<List<User>> GetAllUsers()
    {
        var response = await _factory.CreateClient().GetAsync("api/v1/User");
        string jsonResult = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<User>>(jsonResult)!;
    }
}