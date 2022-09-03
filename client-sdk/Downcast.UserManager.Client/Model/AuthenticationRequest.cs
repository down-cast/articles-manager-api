namespace Downcast.UserManager.Client.Model;

public class AuthenticationRequest : UpdatePasswordInput
{
    public string Email { get; init; } = null!;
}