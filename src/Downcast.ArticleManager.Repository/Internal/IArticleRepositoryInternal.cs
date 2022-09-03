using Downcast.ArticleManager.Repository.Domain;
using Downcast.ArticleManager.Repository.Domain.Enums;
using Downcast.ArticleManager.Repository.InputModels;

namespace Downcast.ArticleManager.Repository.Internal;

internal interface IArticleRepositoryInternal
{
    Task<Article> GetArticle(string id);
    IAsyncEnumerable<Article> GetArticlesByUserId(string userId, ArticlesFilter filter);
    Task<string> AddArticle(CreateArticle article);
    Task UpdateArticleAsync(string id, UpdateArticle article);
    Task DeleteArticle(string id);
    Task SetArticleState(string id, ArticleState state);
}