namespace Downcast.ArticleManager.Repository.InputModels;

public class UpdateArticle
{
    public string? Title { get; init; }

    public string? Description { get; init; }

    public string? ThumbnailPictureUri { get; init; }

    public TimeSpan? TimeToRead { get; init; }

    public IEnumerable<string>? Keywords { get; init; }

    public string? HtmlUri { get; set; }
}