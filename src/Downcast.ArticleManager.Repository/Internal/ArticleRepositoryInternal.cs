using Downcast.ArticleManager.Repository.Domain;
using Downcast.ArticleManager.Repository.Domain.Enums;
using Downcast.ArticleManager.Repository.Filters;
using Downcast.Common.Errors;

using Firestore.Typed.Client;
using Firestore.Typed.Client.Extensions;

using Google.Cloud.Firestore;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Downcast.ArticleManager.Repository.Internal;

internal class ArticleRepositoryInternal : IArticleRepositoryInternal
{
    private readonly ILogger<ArticleRepositoryInternal> _logger;
    private readonly FirestoreDb _firestoreDb;
    private readonly TypedCollectionReference<Article> _collection;

    public ArticleRepositoryInternal(
        IOptions<RepositoryOptions> options,
        ILogger<ArticleRepositoryInternal> logger,
        FirestoreDb firestoreDb)
    {
        _logger = logger;
        _firestoreDb = firestoreDb;
        _collection = firestoreDb.TypedCollection<Article>(options.Value.ArticlesCollection);
    }

    /// <summary>
    /// Returns an article by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="DcException"></exception>
    public async Task<Article> GetArticle(string id)
    {
        TypedDocumentSnapshot<Article> snapshot = await _collection
            .Document(id)
            .GetSnapshotAsync()
            .ConfigureAwait(false);

        if (snapshot.Exists)
        {
            return snapshot.RequiredObject;
        }

        _logger.LogInformation("Article with id {Id} not found", id);
        throw new DcException(ErrorCodes.EntityNotFound);
    }

    /// <summary>
    /// Returns user articles, ordered by most recent creation date
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<Article> GetArticlesByUserId(string userId, ArticlesFilter filter)
    {
        TypedQuery<Article> query = _collection.WhereEqualTo(a => a.AuthorUserId, userId);

        if (filter.State.HasValue)
        {
            query = query.WhereEqualTo(a => a.State, filter.State);
        }

        IAsyncEnumerable<TypedDocumentSnapshot<Article>> articles = query
            .OrderByDescending(a => a.Created)
            .Offset(filter.Skip)
            .Limit(filter.Top)
            .StreamAsync();

        await foreach (TypedDocumentSnapshot<Article> article in articles.ConfigureAwait(false))
        {
            yield return article.RequiredObject;
        }
    }

    public Task<Article> AddArticleAsync(Article article)
    {
        throw new NotImplementedException();
    }

    public Task<Article> UpdateArticleAsync(Article article)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Deletes article by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task DeleteArticle(string id)
    {
        return _collection.Document(id).DeleteAsync();
    }

    /// <summary>
    /// Sets article status
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public Task SetArticleState(string id, ArticleState state)
    {
        return _collection.Document(id).UpdateAsync(article => article.State, state);
    }
}