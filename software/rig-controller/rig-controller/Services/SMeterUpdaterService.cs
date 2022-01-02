using Microsoft.AspNetCore.SignalR;
using rig_controller.Hubs;

namespace rig_controller.Services
{
    public class SMeterUpdaterService : IHostedService
    {
        private Timer? timer;
        private readonly Random random = new();
        private readonly IHubContext<UiHub> uiHubContext;

        public SMeterUpdaterService(IHubContext<UiHub> uiHubContext)
        {
            this.uiHubContext = uiHubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(Tick, null, 0, 5000);

            return Task.CompletedTask;
        }

        private void Tick(object? state)
        {
            Task.Run(async () => await uiHubContext.Clients.All.SendAsync(nameof(UiHub.SetSMeter), random.Next(0, 100)));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
