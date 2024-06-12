using System.Diagnostics.CodeAnalysis;

namespace fastfood_payment.Infra.Email.Configuration;

[ExcludeFromCodeCoverage]
public class EmailSettings
{
    public string Username { get; set; }
    public string Password { get; set; }
}
