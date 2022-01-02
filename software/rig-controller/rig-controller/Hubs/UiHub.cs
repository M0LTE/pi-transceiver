using Microsoft.AspNetCore.SignalR;
using rig_controller.Services;

namespace rig_controller.Hubs
{
    public class UiHub : Hub
    {
        private readonly ILogger<UiHub> logger;

        public UiHub(ILogger<UiHub> logger)
        {
            this.logger = logger;
        }

        public async Task SetFrequencyDigit(int digitNumber, int newValue)
        {
            logger.LogInformation($"UI set digit {digitNumber} to {newValue}");

            // just to prove a point
            await Clients.All.SendAsync(nameof(SetSMeter), newValue);
        }

        internal async Task SetSMeter(int value)
        {
            logger.LogInformation($"Server setting S meter in UI to {value}");

            await Clients.All.SendAsync(nameof(SetSMeter), value);
        }
    }
}
