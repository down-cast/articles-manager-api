namespace Downcast.UserManager.Client.Model;

public class CreateUserInputModel
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string? DisplayName { get; init; }
}