using Microsoft.Extensions.Options;

namespace rig_controller.Services
{
    public class StartupService : IHostedService
    {
        private readonly GpioService gpioService;
        private readonly RigOptions rigOptions;

        public StartupService(GpioService gpioService, IOptions<RigOptions> options)
        {
            this.gpioService = gpioService;
            rigOptions = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await gpioService.SetGpio(rigOptions.RXTX_CHANGEOVER_RELAY_PIN, true);
            await gpioService.SetGpio(rigOptions.PA_RELAY_PIN, true);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
