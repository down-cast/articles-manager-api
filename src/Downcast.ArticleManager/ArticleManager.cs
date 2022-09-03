using Downcast.ArticleManager.Repository;

using Microsoft.Extensions.Logging;

namespace Downcast.ArticleManager;

public class ArticleManager : IArticleManager
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<ArticleManager> _logger;

    public ArticleManager(IArticleRepository articleRepository, ILogger<ArticleManager> logger)
    {
        _articleRepository = articleRepository;
        _logger = logger;
    }
}