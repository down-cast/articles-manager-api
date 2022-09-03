using System.ComponentModel.DataAnnotations;

namespace Downcast.ArticleManager.Repository.Internal;

public class RepositoryOptions
{
    public const string SectionName = "RepositoryOptions";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Collection name cannot be null or empty")]
    public string ArticlesCollection { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "ProjectId is required")]
    public string ProjectId { get; set; } = null!;
}