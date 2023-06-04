namespace Integration.Shared;

public class AsyncLocalStorage<T> where T : class
{
    private static readonly AsyncLocal<T> _asyncLocal = new();

    public static T Current
    {
        get => _asyncLocal.Value!;
        set => _asyncLocal.Value = value;
    }
}