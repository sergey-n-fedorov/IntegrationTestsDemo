using Example.Services.External;
using Moq;

namespace Example.IntegrationTests;

public class MockSetup
{
    public readonly Mock<IExternalServiceClient> ExternalServiceClient = new();
    
    public MockSetup()
    {
        ResetAndSetup();
    }
    
    public void ResetAndSetup()
    {
        ExternalServiceClient.Reset();
        
        ExternalServiceClient.Setup(w => w.GetAllUsersAsync()).ReturnsAsync(TestDataProvider.ExternalUsers);
    }
}