using Microsoft.Extensions.Options;

namespace rig_controller.Services
{
    public class StartupService : IHostedService
    {
        private readonly GpioService _gpioService;
        private readonly IAdcChannelReaderService _adcChannelReaderService;
        private readonly ILogger<StartupService> _logger;
        private readonly RigOptions _rigOptions;
        private Timer? _timer;

        public StartupService(GpioService gpioService, IOptions<RigOptions> options, IAdcChannelReaderService adcChannelReaderService, ILogger<StartupService> logger)
        {
            _gpioService = gpioService;
            _adcChannelReaderService = adcChannelReaderService;
            _logger = logger;
            _rigOptions = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _gpioService.SetGpio(_rigOptions.RXTX_CHANGEOVER_RELAY_PIN, true);
            await _gpioService.SetGpio(_rigOptions.PA_RELAY_PIN, true);

            _timer = new Timer(Tick, null, 0, 1000);
        }

        private async void Tick(object? state)
        {
            try
            {
                var reading = await _adcChannelReaderService.Read(0, 0);
                _logger.LogInformation($"{reading.Millivolts / 1000.0:0.000}V");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading ADC");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }
    }
}