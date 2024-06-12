using fastfood_payment.Domain.Contracts.Email;
using fastfood_payment.Infra.Email.Configuration;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace fastfood_payment.Infra.Email;

[ExcludeFromCodeCoverage]
public class EmailClient(IOptions<EmailSettings> options) : IEmailClient
{
    private readonly EmailSettings _settings = options.Value ?? throw new ArgumentNullException(nameof(options));

    public async Task SendEmailAsync(string subject, string body, string recipients)
    {
        SmtpClient smtpClient = new()
        {
            Port = 587,
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true,
            Host = "smtp.mailersend.net"
        };

        MailMessage message = new()
        {
            From = new MailAddress("MS_BJUx2P@trial-v69oxl5okjxg785k.mlsender.net"),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        message.To.Add(new MailAddress(recipients));

        smtpClient.Send(message);
    }
}
