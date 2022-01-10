namespace rig_controller.Services
{
    public class StartupService : IHostedService
    {
        private readonly PttService pttService;

        public StartupService(PttService pttService)
        {
            this.pttService = pttService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await pttService.Unkey();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
