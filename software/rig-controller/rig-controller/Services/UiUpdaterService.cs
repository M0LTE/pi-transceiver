using Microsoft.AspNetCore.SignalR;
using rig_controller.Hubs;

namespace rig_controller.Services
{
    public class UiUpdaterService : IHostedService
    {
        private Timer? timer;
        private readonly Random random = new();
        private readonly IHubContext<UiHub> uiHubContext;
        private readonly ILogger<UiUpdaterService> logger;

        public UiUpdaterService(IHubContext<UiHub> uiHubContext, ILogger<UiUpdaterService> logger)
        {
            this.uiHubContext = uiHubContext;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(Tick, null, 0, 5000);

            return Task.CompletedTask;
        }

        private void Tick(object? state)
        {
            Task.Run(async () => await SetSMeter(random.Next(0, 100)));

            Task.Run(async () => await SetFrequency(random.Next(144000000, 146000000)));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async Task SetSMeter(int value)
        {
            logger.LogInformation($"Server setting S meter in UI to {value}");

            await uiHubContext.Clients.All.SendAsync(nameof(SetSMeter), value);
        }

        private async Task SetFrequency(long hz)
        {
            if (hz < 0 || hz > 3000000000L)
            {
                throw new ArgumentOutOfRangeException(nameof(hz));
            }

            string digits = (hz / 1000000.0).ToString("0000.000");

            logger.LogInformation($"Server setting frequency in UI to {digits}");

            await uiHubContext.Clients.All.SendAsync(nameof(SetFrequency), digits[0], digits[1], digits[2], digits[3], digits[5], digits[6], digits[7]);
        }
    }
}
