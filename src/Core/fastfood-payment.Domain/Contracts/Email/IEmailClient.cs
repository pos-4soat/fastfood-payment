namespace fastfood_payment.Domain.Contracts.Email;

public interface IEmailClient
{
    Task SendEmailAsync(string subject, string body, string recipients);
}
