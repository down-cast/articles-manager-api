using Downcast.ArticleManager.Repository.Domain;
using Downcast.ArticleManager.Repository.Domain.Enums;
using Downcast.ArticleManager.Repository.Filters;

namespace Downcast.ArticleManager.Repository.Internal;

internal interface IArticleRepositoryInternal
{
    Task<Article> GetArticle(string id);
    IAsyncEnumerable<Article> GetArticlesByUserId(string userId, ArticlesFilter filter);
    Task<Article> AddArticleAsync(Article article);
    Task<Article> UpdateArticleAsync(Article article);
    Task DeleteArticle(string id);
    Task SetArticleState(string id, ArticleState state);
}