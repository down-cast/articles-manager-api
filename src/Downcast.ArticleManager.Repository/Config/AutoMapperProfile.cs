using AutoMapper;

namespace Downcast.ArticleManager.Repository.Config;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Allow to map public and internal properties
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        ShouldMapProperty = arg => arg?.GetMethod?.IsPublic is true || arg?.GetMethod?.IsAssembly is true;
    }
}