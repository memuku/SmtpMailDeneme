﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmtpMailDeneme.Configuration;
using SmtpMailDeneme.Data;
using SmtpMailDeneme.Services;

namespace SmtpMailDeneme.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService = null;
        private readonly IVerificationCodeService _verificationCodeService;
        private string savedVerificationCode;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public bool SendEmail(EmailData emailData)
        {
            return _emailService.SendEmail(emailData);
        }
        [HttpPost("SendVerificationCode")]
        public IActionResult SendVerificationCode(string userEmail)
        {
            string verificationCode = _emailService.GenerateRandomCode(); // Rastgele 4 haneli kod oluşturuyoruz

            // Onay kodunu kullanıcının e-posta adresine gönder
            EmailData emailData = new EmailData
            {
                EmailToId = userEmail,
                EmailToName = "Kullanıcı",
                EmailSubject = "Onay Kodu",
                EmailBody = $"Onay kodunuz: {verificationCode}"
            };

            _emailService.SendEmail(emailData);

            // Onay kodunu döndür
            return Ok(verificationCode);
        }
        [HttpGet("CheckVerificationCode")]
        public IActionResult CheckVerificationCode(string userEmail, string verificationCode)
        {
            bool isVerificationCodeValid = _verificationCodeService.CheckVerificationCode(userEmail, verificationCode);

            // Doğrulama kodunun doğru olup olmadığına göre sonucu döndürün:
            return Ok(isVerificationCodeValid);
        }
    }

}

