using Microsoft.AspNetCore.SignalR;
using rig_controller.Services;

namespace rig_controller.Hubs
{
    public class UiHub : Hub
    {
        private readonly UiUpdaterService uiUpdaterService;
        private readonly RigStateService rigStateService;

        public UiHub(UiUpdaterService uiUpdaterService, RigStateService rigStateService)
        {
            this.uiUpdaterService = uiUpdaterService;
            this.rigStateService = rigStateService;
        }

        public async Task SetFrequencyDigit(int digitNumber, bool up)
        {
            var pow = 10 - digitNumber;

            var value = Math.Pow(10, pow);

            var newFrequency = (long)(rigStateService.RigState.Frequency + ((up ? 1 : -1) * value));

            rigStateService.RigState.Frequency = newFrequency;

            await uiUpdaterService.SetFrequency();
        }
    }
}