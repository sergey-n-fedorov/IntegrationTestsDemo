namespace Integration.IntegrationTests.Tools;

public class AsyncLocalStorage<T> where T : class
{
    private static readonly AsyncLocal<T> AsyncLocal = new();

    public static T Current
    {
        get => AsyncLocal.Value!;
        set => AsyncLocal.Value = value;
    }
}