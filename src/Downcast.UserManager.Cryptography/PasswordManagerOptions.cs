using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Cryptography;

public class PasswordManagerOptions : IValidatableObject
{
    public const string SectionName = "PasswordOptions";

    [Required]
    [Range(10000, int.MaxValue)]
    public int Iterations { get; init; } = 30000;

    [Required]
    public int SaltSizeInBytes { get; init; } = 64;

    [Required]
    public int HashSizeInBytes { get; init; } = 512;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SaltSizeInBytes % 8 is not 0)
        {
            yield return new ValidationResult("Salt size must be a multiple of 8", new[] { nameof(SaltSizeInBytes) });
        }

        if (HashSizeInBytes % 8 is not 0)
        {
            yield return new ValidationResult("Hash size must be a multiple of 8", new[] { nameof(HashSizeInBytes) });
        }
    }
}