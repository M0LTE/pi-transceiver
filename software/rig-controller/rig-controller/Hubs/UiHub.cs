using Microsoft.AspNetCore.SignalR;
using rig_controller.Services;

namespace rig_controller.Hubs
{
    public class UiHub : Hub
    {
        private readonly UiUpdaterService uiUpdaterService;
        private readonly RigStateService rigStateService;
        private readonly PttService pttService;
        private readonly ILogger<UiHub> logger;

        public UiHub(UiUpdaterService uiUpdaterService, RigStateService rigStateService, PttService pttService, ILogger<UiHub> logger)
        {
            this.uiUpdaterService = uiUpdaterService;
            this.rigStateService = rigStateService;
            this.pttService = pttService;
            this.logger = logger;
        }

        public async Task SetFrequencyDigit(int digitNumber, bool up)
        {
            var pow = 10 - digitNumber;

            var value = Math.Pow(10, pow);
            var add = ((up ? 1 : -1) * value);

            var newFrequency = (long)(rigStateService.RigState.Frequency + add);

            if (newFrequency > 0 && newFrequency < 3000000000L)
            {
                logger.LogInformation("Adding " + add);

                rigStateService.RigState.Frequency = newFrequency;

                await uiUpdaterService.SetFrequency();
            }
        }

        public async Task TriggerFrequencyUpdate()
        {
            await uiUpdaterService.SetFrequency();
        }

        public async Task ToggleTx()
        {
            if (rigStateService.RigState.Transmitting)
            {
                await pttService.Unkey();
            }
            else
            {
                await pttService.Key();
            }
        }
    }
}