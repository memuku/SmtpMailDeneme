namespace SmtpMailDeneme.Services
{
    public interface IVerificationCodeService
    {
        void SaveVerificationCode(string userEmail, string verificationCode);
        bool CheckVerificationCode(string userEmail, string verificationCode);
        void DeleteVerificationCodesOlderThan(DateTime expirationTime);
    }
}
