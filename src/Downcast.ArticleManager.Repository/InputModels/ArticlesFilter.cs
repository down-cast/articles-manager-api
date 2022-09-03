using Downcast.ArticleManager.Repository.Domain.Enums;

namespace Downcast.ArticleManager.Repository.InputModels;

internal class ArticlesFilter
{
    public int Top { get; init; }
    public int Skip { get; init; }
    public ArticleState? State { get; init; }
}