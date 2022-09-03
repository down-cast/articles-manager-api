using Downcast.Common.HttpClient.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Refit;

namespace Downcast.UserManager.Client.DependencyInjection;

public static class UserManagerClientConfigExtensions
{
    public static IServiceCollection AddUserManagerClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationSectionName = "UserManagerClient")
    {
        services.AddRefitClient<IUserManagerClient>()
            .ConfigureDowncastHttpClient(configuration, configurationSectionName);
        return services;
    }
}