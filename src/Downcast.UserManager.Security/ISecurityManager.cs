using Downcast.UserManager.Model.Input;

namespace Downcast.UserManager.Security;

public interface ISecurityManager
{
    Task<bool> ValidateCredentials(AuthenticationRequest auth);
    Task UpdatePassword(string userId, UpdatePasswordInput passwordInput);
}