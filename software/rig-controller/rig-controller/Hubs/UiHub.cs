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

        public Task SetFrequencyDigit(int digitNumber, int newValue)
        {
            logger.LogInformation($"UI set digit {digitNumber} to {newValue}");

            return Task.CompletedTask;
        }
    }
}
