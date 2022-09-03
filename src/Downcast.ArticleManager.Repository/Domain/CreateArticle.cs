using Downcast.ArticleManager.Repository.Domain.Enums;

using Google.Cloud.Firestore;

namespace Downcast.ArticleManager.Repository.Domain;

[FirestoreData]
internal class CreateArticle
{
    [FirestoreProperty]
    public string AuthorUserId { get; init; } = null!;

    [FirestoreProperty]
    public ArticleState State { get; init; }

    [FirestoreProperty]
    public string Title { get; init; } = string.Empty;

    [FirestoreProperty]
    public string Description { get; init; } = string.Empty;

    [FirestoreProperty]
    public string ThumbnailPictureUri { get; init; } = string.Empty;

    [FirestoreProperty]
    public TimeSpan TimeToRead { get; init; }

    [FirestoreProperty]
    public IEnumerable<string> Keywords { get; init; } = Enumerable.Empty<string>();

    [FirestoreProperty]
    public string HtmlUri { get; set; } = string.Empty;
}