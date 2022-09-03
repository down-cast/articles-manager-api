namespace Downcast.UserManager.Client.Model;

public class User
{
    public string Id { get; init; } = null!;
    public string? DisplayName { get; init; }
    public Uri? ProfilePictureUri { get; init; }
    public SocialLinks? SocialLinks { get; init; }
    public bool EmailValidated { get; init; }
    public string? Description { get; init; }
    public string Email { get; init; } = null!;
    public IEnumerable<string> Roles { get; init; } = Enumerable.Empty<string>();
    public bool HasPicture { get; init; }
    public bool HasPassword { get; init; }
    public DateTime Created { get; init; }
    public DateTime Updated { get; init; }
    public bool HasRole(string role) => Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
}