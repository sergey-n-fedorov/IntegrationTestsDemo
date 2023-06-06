using DotNet.Testcontainers.Containers;

namespace Integration.IntegrationTests.Tools.TestContainers;

public static class DockerContainerExtensions
{
    internal static async Task ExecWithExceptionAsync(this DockerContainer container, List<string> splitCommand, CancellationToken ct = default)
    {
        var result = await container.ExecAsync(splitCommand, ct);

        if (result.ExitCode != 0) {
            throw new Exception($"Command {string.Join(" ",splitCommand)} failed with exit code {result.ExitCode}. Output: {result.Stderr}");
        }
    }
}