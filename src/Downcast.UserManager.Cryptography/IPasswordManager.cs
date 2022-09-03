using Downcast.UserManager.Model;

namespace Downcast.UserManager.Cryptography;

public interface IPasswordManager
{
    bool VerifyPassword(string password, PasswordInfo passwordInfo);
    PasswordInfo HashPassword(string password);
    bool IsPasswordSecurityOutdated(PasswordInfo passwordInfo);
}