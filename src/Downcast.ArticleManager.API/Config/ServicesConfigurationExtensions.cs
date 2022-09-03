using Downcast.ArticleManager.Repository.Config;

namespace Downcast.ArticleManager.API.Config;

public static class ServicesConfigurationExtensions
{
    public static WebApplicationBuilder AddArticleManagerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddArticleRepository(builder.Configuration);
        return builder;
    }
}