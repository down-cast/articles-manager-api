using AutoMapper;

using Downcast.ArticleManager.Repository.Internal;

namespace Downcast.ArticleManager.Repository;

internal class ArticleRepository : IArticleRepository
{
    private readonly IMapper _mapper;
    private readonly IArticleRepositoryInternal _repo;

    public ArticleRepository(IMapper mapper, IArticleRepositoryInternal repo)
    {
        _mapper = mapper;
        _repo = repo;
    }
}