namespace SmtpMailDeneme.Services
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly MongoDBService<VerificationCode> _verificationCodeService;


        public VerificationCodeService(MongoDBService<VerificationCode> mongoDBService)
        {
            _verificationCodeService = mongoDBService;
            _verificationCodeService = new MongoDBService<VerificationCode>();
        }

        public void SaveVerificationCode(string userEmail, string verificationCode)
        {
            var code = new VerificationCode
            {
                UserEmail = userEmail,
                Code = verificationCode,
                CreatedAt = DateTime.UtcNow
            };

            _verificationCodeService.Add(code);
        }

        public bool CheckVerificationCode(string userEmail, string verificationCode)
        {
            var code = _verificationCodeService.Get(x => x.UserEmail == userEmail && x.Code == verificationCode);

            if (code != null)
            {
                return true;
            }

            return false;
        }

        public void DeleteVerificationCodesOlderThan(DateTime expirationTime)
        {
            var filter = Builders<VerificationCode>.Filter.Lt("CreatedAt", expirationTime);
            _verificationCodeService.Delete(filter);
        }
    }
}
