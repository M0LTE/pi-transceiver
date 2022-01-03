namespace rig_controller.Services
{
    public class UiUpdaterHostedService : IHostedService
    {
        private Timer? timer;
        private readonly UiUpdaterService uiUpdaterService;

        public UiUpdaterHostedService(UiUpdaterService uiUpdaterService)
        {
            this.uiUpdaterService = uiUpdaterService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(Tick, null, 0, 500);
            return Task.CompletedTask;
        }

        private void Tick(object? state)
        {
            Task.Run(async () =>
            {
                await uiUpdaterService.SetFrequency();
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            return Task.CompletedTask;
        }
    }
}
