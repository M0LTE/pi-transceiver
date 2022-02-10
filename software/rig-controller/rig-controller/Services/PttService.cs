using Microsoft.Extensions.Options;

namespace rig_controller.Services
{
    public class PttService
    {
        private readonly RigStateService rigStateService;
        private readonly GpioService gpioService;
        private readonly IFlowgraphControlService flowgraphControlService;
        private readonly UiUpdaterService uiUpdaterService;
        private readonly IOptionsMonitor<RigOptions> rigOptionsMonitor;

        private const bool RelayEnergisedGpioState = false;
        private const bool RelayDeenergisedGpioState = true;

        public PttService(RigStateService rigStateService, GpioService gpioService, IFlowgraphControlService flowgraphControlService, UiUpdaterService uiUpdaterService, IOptionsMonitor<RigOptions> rigOptionsMonitor)
        {
            this.rigStateService = rigStateService;
            this.gpioService = gpioService;
            this.flowgraphControlService = flowgraphControlService;
            this.uiUpdaterService = uiUpdaterService;
            this.rigOptionsMonitor = rigOptionsMonitor;
        }

        private RigOptions rigOptions => rigOptionsMonitor.CurrentValue;

        internal async Task Key()
        {
            await uiUpdaterService.AddLogLine("TX requested");

            if (rigStateService.RigState.Transmitting == true)
            {
                return;
            }

            // Rx to Tx:  Ant relay to Tx, delay 20ms (Trelay=15 ms max), PA bias on, unmute flowgraph

            await gpioService.SetGpio(rigOptions.RXTX_CHANGEOVER_RELAY_PIN, RelayEnergisedGpioState);
            await Task.Delay(rigOptions.RXTX_RELAY_DELAY);
            await gpioService.SetGpio(rigOptions.PA_RELAY_PIN, RelayEnergisedGpioState);
            await flowgraphControlService.UnmuteAudioSource();

            rigStateService.RigState.Transmitting = true;
            await uiUpdaterService.AddLogLine("TX set");
        }

        public async Task Unkey()
        {
            await uiUpdaterService.AddLogLine("RX requested");

            if (rigStateService.RigState.Transmitting == false)
            {
                return;
            }

            // Tx to Rx:  Mute flowgraph. PA bias off, delay 20ms + time it takes to mute flowgraph max. Ant relay to Rx 

            await flowgraphControlService.MuteAudioSource();
            await gpioService.SetGpio(rigOptions.PA_RELAY_PIN, RelayDeenergisedGpioState);
            await Task.Delay(rigOptions.PA_RELAY_DELAY);
            await Task.Delay(rigOptions.FLOWGRAPH_DELAY);
            await gpioService.SetGpio(rigOptions.RXTX_CHANGEOVER_RELAY_PIN, RelayDeenergisedGpioState);

            rigStateService.RigState.Transmitting = false;
            await uiUpdaterService.AddLogLine("RX set");
        }
    }
}
