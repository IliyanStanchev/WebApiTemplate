using System.ComponentModel.DataAnnotations;

namespace WebApiTemplate.Domain.Options;

public class AuthenticationOptions
{
    public static string SectionName => nameof(AuthenticationOptions);

    [Required]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;
}
