using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpMailDeneme.Configuration;
using SmtpMailDeneme.Data;

namespace SmtpMailDeneme.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public bool SendEmail(EmailData emailData)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();

                MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = emailData.EmailSubject;
                string verificationCode = GenerateRandomCode();
                emailData.EmailBody += $"\nOnay kodunuz: {verificationCode}";

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = emailData.EmailBody;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                //Log Exception Details
                return false;
            }     
        }
        public bool SendVerificationEmail(EmailData emailModel)
        {
            try
            {
                string verificationCode = GenerateRandomCode();
                emailModel.EmailBody += $"\nOnay kodunuz: {verificationCode}";

                MimeMessage emailMessage = new MimeMessage();

                MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(emailModel.EmailToName, emailModel.EmailToId);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = emailModel.EmailSubject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = emailModel.EmailBody;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                MailKit.Net.Smtp.SmtpClient emailClient = new MailKit.Net.Smtp.SmtpClient();
                emailClient.Connect(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                // Log Exception Details
                return false;
            }
        }

        public string GenerateRandomCode()
        {
            Random random = new Random();
            int code = random.Next(1000, 9999); // 1000 ile 9999 arasında rastgele bir sayı üretiyoruz (4 haneli)
            return code.ToString();
        }
    }
}
