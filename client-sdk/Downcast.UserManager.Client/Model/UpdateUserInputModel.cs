namespace Downcast.UserManager.Client.Model;

public class UpdateUserInputModel
{ 
    public string? DisplayName { get; init; }
    public string? ProfilePictureUri { get; init; }
    public SocialLinks? SocialLinks { get; init; }
    public bool? EmailValidated { get; init; }
    public string? Description { get; init; }
}