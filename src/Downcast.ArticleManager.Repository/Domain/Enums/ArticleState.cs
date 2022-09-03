using Google.Cloud.Firestore;

namespace Downcast.ArticleManager.Repository.Domain.Enums;

[FirestoreData]
internal enum ArticleState
{
    Draft = 0,
    AwaitApproval = 1,
    Published = 2,
}