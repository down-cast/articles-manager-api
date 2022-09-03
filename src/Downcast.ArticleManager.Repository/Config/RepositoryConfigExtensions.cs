using Downcast.ArticleManager.Repository.Internal;

using Google.Api.Gax;
using Google.Cloud.Firestore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Downcast.ArticleManager.Repository.Config;

public static class RepositoryConfigExtensions
{
    public static IServiceCollection AddArticleRepository(this IServiceCollection services, IConfiguration configuration)
    {
        AddMapper(services);
        AddFirestoreDb(services);

        services
            .AddOptions<RepositoryOptions>()
            .Bind(configuration.GetSection(RepositoryOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IArticleRepository, ArticleRepository>();
        services.AddSingleton<IArticleRepositoryInternal, ArticleRepositoryInternal>();

        return services;
    }

    private static void AddMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));
    }

    private static void AddFirestoreDb(IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            IOptions<RepositoryOptions> options = provider.GetRequiredService<IOptions<RepositoryOptions>>();
            return new FirestoreDbBuilder
            {
                ProjectId = options.Value.ProjectId,
                EmulatorDetection = EmulatorDetection.EmulatorOrProduction
            }.Build();
        });
    }
}