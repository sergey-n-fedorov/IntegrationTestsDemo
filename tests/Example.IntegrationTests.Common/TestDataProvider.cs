using Example.Services.External;
using Newtonsoft.Json;

namespace Example.IntegrationTests;

public static class TestDataProvider
{
    public static readonly List<ExternalUser> ExternalUsers = JsonConvert.DeserializeObject<List<ExternalUser>>(File.ReadAllText(@"TestData\Users.json"))!;
}