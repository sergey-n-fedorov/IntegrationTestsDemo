namespace Example.IntegrationTests.TestContext;

public class IntegrationTestContext
{
    private static readonly AsyncLocal<IntegrationTestContext?> AsyncLocal = new();

    public static IntegrationTestContext? Current
    {
        get => AsyncLocal.Value;
        set => AsyncLocal.Value = value;
    }

    public bool StateChanged { get; set; }
}
