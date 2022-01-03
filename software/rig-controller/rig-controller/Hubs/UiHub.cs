using Microsoft.AspNetCore.SignalR;
using rig_controller.Services;

namespace rig_controller.Hubs
{
    public class UiHub : Hub
    {
        private readonly ILogger<UiHub> logger;
        private readonly RigStateService rigStateService;
        private readonly UiUpdaterService uiUpdaterService;

        public UiHub(ILogger<UiHub> logger, RigStateService rigStateService, UiUpdaterService uiUpdaterService)
        {
            this.logger = logger;
            this.rigStateService = rigStateService;
            this.uiUpdaterService = uiUpdaterService;
        }

        public async Task SetFrequencyDigit(int digitNumber, bool up)
        {
            var pow = 10 - digitNumber;

            var value = Math.Pow(10, pow);

            var newFrequency = (long)(rigStateService.RigState.Frequency + ((up ? 1 : -1) * value));

             await uiUpdaterService.SetFrequency(newFrequency);
            rigStateService.RigState.Frequency = newFrequency;
        }
    }
}
