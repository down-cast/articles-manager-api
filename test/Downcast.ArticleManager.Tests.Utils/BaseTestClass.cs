using Xunit;

namespace Downcast.ArticleManager.Tests.Utils;

public class BaseTestClass : IAsyncLifetime
{
    private readonly ArticleManagerServerInstance _server = new();

    public BaseTestClass()
    {
        HttpClient httpClient = _server.CreateClient();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _server.DisposeAsync().ConfigureAwait(false);
    }
}