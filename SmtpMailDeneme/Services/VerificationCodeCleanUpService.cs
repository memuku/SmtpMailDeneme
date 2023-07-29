namespace SmtpMailDeneme.Services
{
    public class VerificationCodeCleanupService : IHostedService, IDisposable
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly TimeSpan _interval;
        private Timer _timer;

        public VerificationCodeCleanupService(IVerificationCodeService verificationCodeService)
        {
            _verificationCodeService = verificationCodeService;
            _interval = TimeSpan.FromMinutes(3); // 3 dakika 
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, _interval);
            return Task.CompletedTask;
        }
        private void DeleteOldVerificationCodes()
        {
            var expirationTime = DateTime.UtcNow.AddMinutes(-3);
            _verificationCodeService.DeleteVerificationCodesOlderThan(expirationTime);
        }
        private void DoWork(object state)
        {

            DeleteOldVerificationCodes();
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
