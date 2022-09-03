using Downcast.ArticleManager.API.Controllers;
using Downcast.ArticleManager.Repository.Internal;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Downcast.ArticleManager.Tests.Utils;

public class ArticleManagerServerInstance : WebApplicationFactory<ArticleController>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            services.Configure<RepositoryOptions>(options =>
            {
                options.ArticlesCollection = "articles_test";
                options.ProjectId = ProjectId;
            });
        });
    }

    private static string ProjectId => Environment.GetEnvironmentVariable("FIRESTORE_PROJECT_ID") ??
                                       throw new InvalidOperationException(
                                           "FIRESTORE_PROJECT_ID environment variable is not set");
}