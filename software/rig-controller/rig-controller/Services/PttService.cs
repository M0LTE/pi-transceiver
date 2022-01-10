using Microsoft.Extensions.Options;
using static rig_controller.Services.TimedLock;

namespace rig_controller.Services
{
    public class PttService
    {
        private readonly RigStateService rigStateService;
        private readonly GpioService gpioService;
        private readonly FlowgraphControlService flowgraphControlService;
        private readonly UiUpdaterService uiUpdaterService;
        private readonly IOptionsMonitor<RigOptions> rigOptionsMonitor;

        public PttService(RigStateService rigStateService, GpioService gpioService, FlowgraphControlService flowgraphControlService, UiUpdaterService uiUpdaterService, IOptionsMonitor<RigOptions> rigOptionsMonitor)
        {
            this.rigStateService = rigStateService;
            this.gpioService = gpioService;
            this.flowgraphControlService = flowgraphControlService;
            this.uiUpdaterService = uiUpdaterService;
            this.rigOptionsMonitor = rigOptionsMonitor;
        }

        private RigOptions rigOptions => rigOptionsMonitor.CurrentValue;

        private static object pttGuard = new object();

        internal async Task Key()
        {
            using (Lock(pttGuard))
            {
                if (rigStateService.RigState.Transmitting == true)
                {
                    return;
                }

                // Rx to Tx:  Ant relay to Tx, delay 20ms (Trelay=15 ms max), PA bias on, unmute flowgraph

                await gpioService.SetGpio(rigOptions.RXTX_CHANGEOVER_RELAY_PIN, true);
                await Task.Delay(rigOptions.RXTX_RELAY_DELAY);
                await gpioService.SetGpio(rigOptions.PA_RELAY_PIN, true);
                await flowgraphControlService.UnmuteAudioSource();

                rigStateService.RigState.Transmitting = true;
                await uiUpdaterService.AddLogLine("TX set");
            }
        }

        public async Task Unkey()
        {
            using (Lock(pttGuard))
            {
                if (rigStateService.RigState.Transmitting == false)
                {
                    return;
                }

                // Tx to Rx:  Mute flowgraph. PA bias off, delay 20ms + time it takes to mute flowgraph max. Ant relay to Rx 

                await flowgraphControlService.MuteAudioSource();
                await gpioService.SetGpio(rigOptions.PA_RELAY_PIN, false);
                await Task.Delay(rigOptions.PA_RELAY_DELAY);
                await Task.Delay(rigOptions.FLOWGRAPH_DELAY);
                await gpioService.SetGpio(rigOptions.RXTX_CHANGEOVER_RELAY_PIN, false);

                rigStateService.RigState.Transmitting = false;
                await uiUpdaterService.AddLogLine("RX set");
            }
        }

        private static LockReleaser Lock(object lockObject) => new TimedLock(lockObject).Lock(TimeSpan.FromSeconds(2));
    }

    public class TimedLock
    {
        private readonly object toLock;

        public TimedLock(object toLock)
        {
            this.toLock = toLock;
        }

        public LockReleaser Lock(TimeSpan timeout)
        {
            if (Monitor.TryEnter(toLock, timeout))
            {
                return new LockReleaser(toLock);
            }
            throw new TimeoutException();
        }

        public struct LockReleaser : IDisposable
        {
            private readonly object toRelease;

            public LockReleaser(object toRelease)
            {
                this.toRelease = toRelease;
            }
            public void Dispose()
            {
                Monitor.Exit(toRelease);
            }
        }
    }
}
