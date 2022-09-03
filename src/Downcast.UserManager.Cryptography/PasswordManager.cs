using System.Security.Cryptography;

using Downcast.UserManager.Model;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

namespace Downcast.UserManager.Cryptography;

public class PasswordManager : IPasswordManager
{
    private readonly IOptions<PasswordManagerOptions> _options;
    private const KeyDerivationPrf KeyDerivationAlg = KeyDerivationPrf.HMACSHA512;

    public PasswordManager(IOptions<PasswordManagerOptions> options)
    {
        _options = options;
    }

    /// <summary>
    /// Calculates if the current password is hashed with the current security settings
    /// </summary>
    /// <param name="passwordInfo"></param>
    /// <returns></returns>
    public bool IsPasswordSecurityOutdated(PasswordInfo passwordInfo)
    {
        return passwordInfo.Iterations != _options.Value.Iterations ||
               Convert.FromBase64String(passwordInfo.Hash).Length != _options.Value.HashSizeInBytes ||
               Convert.FromBase64String(passwordInfo.Salt).Length != _options.Value.SaltSizeInBytes;
    }

    /// <summary>
    /// Hashes the password using a randomly generated salt and the configured number of iterations.
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public PasswordInfo HashPassword(string password)
    {
        byte[] salt = GenerateSalt();
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationAlg,
            iterationCount: _options.Value.Iterations,
            numBytesRequested: _options.Value.HashSizeInBytes);

        return new PasswordInfo
        {
            Hash = Convert.ToBase64String(hash),
            Salt = Convert.ToBase64String(salt),
            Iterations = _options.Value.Iterations
        };
    }

    /// <summary>
    /// Verifies the password against the stored password info.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="passwordInfo"></param>
    /// <returns></returns>
    public bool VerifyPassword(string password, PasswordInfo passwordInfo)
    {
        byte[] salt = Convert.FromBase64String(passwordInfo.Salt);
        byte[] hash = Convert.FromBase64String(passwordInfo.Hash);

        byte[] newHash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationAlg,
            iterationCount: passwordInfo.Iterations,
            numBytesRequested: hash.Length);

        return newHash.SequenceEqual(hash);
    }

    /// <summary>
    /// Generates a random salt.
    /// </summary>
    /// <returns></returns>
    private byte[] GenerateSalt() => RandomNumberGenerator.GetBytes(_options.Value.SaltSizeInBytes);
}