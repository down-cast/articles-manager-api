using Google.Cloud.Firestore;

namespace Downcast.ArticleManager.Repository.Domain;

[FirestoreData]
internal class Article : CreateArticle
{
    [FirestoreDocumentId]
    public string Id { get; init; } = null!;

    [FirestoreProperty]
    public DateTime Created { get; init; }

    [FirestoreDocumentUpdateTimestamp]
    public DateTime Updated { get; init; }
}