namespace Example.IntegrationTests.Tools.TestContainers;

internal static class TestContainerTools
{
    public static List<string> SplitCommand(string input)
    {
        return input.Split(" ").Select(w => w.Trim()).Where(w => !string.IsNullOrEmpty(w)).ToList();
    }
}