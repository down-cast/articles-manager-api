using Downcast.ArticleManager.Repository.Domain;
using Downcast.ArticleManager.Repository.Domain.Enums;
using Downcast.ArticleManager.Repository.InputModels;
using Downcast.Common.Errors;

using Firestore.Typed.Client;
using Firestore.Typed.Client.Extensions;

using Google.Cloud.Firestore;

using Grpc.Core;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Downcast.ArticleManager.Repository.Internal;

internal class ArticleRepositoryInternal : IArticleRepositoryInternal
{
    private readonly ILogger<ArticleRepositoryInternal> _logger;
    private readonly TypedCollectionReference<Article> _collection;

    public ArticleRepositoryInternal(
        IOptions<RepositoryOptions> options,
        ILogger<ArticleRepositoryInternal> logger,
        FirestoreDb firestoreDb)
    {
        _logger = logger;
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

    /// <summary>
    /// Creates a new article
    /// </summary>
    /// <param name="article"></param>
    /// <returns>The created article Id</returns>
    public async Task<string> AddArticle(CreateArticle article)
    {
        TypedDocumentReference<Article> document = _collection.Document();
        await document.Untyped.CreateAsync(article).ConfigureAwait(false);
        return document.Id;
    }

    /// <summary>
    /// Updates an article with the fields that are not null in the input model
    /// </summary>
    /// <param name="id"></param>
    /// <param name="article"></param>
    /// <returns></returns>
    public Task UpdateArticleAsync(string id, UpdateArticle article)
    {
        var updateDefinition = new UpdateDefinition<Article>();
        SetDescription(article, updateDefinition);
        SetKeywords(article, updateDefinition);
        SetTitle(article, updateDefinition);
        SetHtmlUri(article, updateDefinition);
        SetTimeToRead(article, updateDefinition);
        SetThumbnailPictureUri(article, updateDefinition);

        return ExecuteDbOperation(() => _collection.Document(id).UpdateAsync(updateDefinition));
    }

    private static void SetThumbnailPictureUri(UpdateArticle article, UpdateDefinition<Article> updateDefinition)
    {
        if (article.ThumbnailPictureUri is not null)
        {
            updateDefinition.Set(a => a.ThumbnailPictureUri, article.ThumbnailPictureUri);
        }
    }

    private static void SetTimeToRead(UpdateArticle article, UpdateDefinition<Article> updateDefinition)
    {
        if (article.TimeToRead is not null)
        {
            updateDefinition.Set(a => a.TimeToRead, article.TimeToRead);
        }
    }

    private static void SetHtmlUri(UpdateArticle article, UpdateDefinition<Article> updateDefinition)
    {
        if (article.HtmlUri is not null)
        {
            updateDefinition.Set(a => a.HtmlUri, article.HtmlUri);
        }
    }

    private static void SetTitle(UpdateArticle article, UpdateDefinition<Article> updateDefinition)
    {
        if (article.Title is not null)
        {
            updateDefinition.Set(a => a.Title, article.Title);
        }
    }

    private static void SetKeywords(UpdateArticle article, UpdateDefinition<Article> updateDefinition)
    {
        if (article.Keywords is not null)
        {
            updateDefinition.Set(a => a.Keywords, article.Keywords);
        }
    }

    private static void SetDescription(UpdateArticle article, UpdateDefinition<Article> updateDefinition)
    {
        if (article.Description is not null)
        {
            updateDefinition.Set(a => a.Description, article.Description);
        }
    }

    private async Task<T> ExecuteDbOperation<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation().ConfigureAwait(false);
        }
        catch (RpcException e)
        {
            _logger.LogError("Database operation failed {StatusCode}, {Message}", e.StatusCode, e.Status.Detail);
            throw new DcException(ErrorCodes.DatabaseError, "Failed to execute operation");
        }
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