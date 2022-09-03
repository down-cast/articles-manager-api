using Downcast.UserManager.Model;

using FluentAssertions;

using Microsoft.Extensions.Options;

namespace Downcast.UserManager.Cryptography.Tests;

public class PasswordCryptographyTests
{
    private readonly IPasswordManager _manager;

    private readonly PasswordManagerOptions _options = new()
    {
        Iterations = 30000,
        SaltSizeInBytes = 16,
        HashSizeInBytes = 256
    };

    public PasswordCryptographyTests()
    {
        _manager = new PasswordManager(Options.Create(_options));
    }

    [Fact]
    public void PasswordHashing_Success()
    {
        const string password = "password1234!";
        PasswordInfo passwordInfo = _manager.HashPassword(password);
        passwordInfo.Iterations.Should().Be(_options.Iterations);

        bool isValid = _manager.VerifyPassword(password, passwordInfo);
        isValid.Should().BeTrue();

        isValid = _manager.VerifyPassword("wrong password", passwordInfo);
        isValid.Should().BeFalse();
    }
}