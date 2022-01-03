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

        public Task SetFrequencyDigit(int digitNumber, bool up)
        {
            return Task.CompletedTask;
        }
    }
}
