using SmtpMailDeneme.Data;

namespace SmtpMailDeneme.Services
{
    public interface IEmailService
    {
        string GenerateRandomCode();
        bool SendEmail(EmailData emailData);
    }
}
