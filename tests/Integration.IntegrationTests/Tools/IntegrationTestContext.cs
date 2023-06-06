namespace Integration.IntegrationTests.Tools;

public class IntegrationTestContext: AsyncLocalStorage<IntegrationTestContext>
{
    public bool StateChanged { get; set; }
}
