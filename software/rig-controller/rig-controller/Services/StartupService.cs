using Microsoft.Extensions.Options;

namespace rig_controller.Services
{
    public class StartupService : IHostedService
    {
        private readonly IGpioService _gpioService;
        private readonly IAdcChannelReaderService _adcChannelReaderService;
        private readonly IPiUpsHatService  _piUpsHatService;
        private readonly ILogger<StartupService> _logger;
        private readonly PlatformInfoProvider platformInfoProvider;
        private readonly RigOptions _rigOptions;
        private Timer? _timer;

        public StartupService(IGpioService gpioService, IOptions<RigOptions> options, IAdcChannelReaderService adcChannelReaderService, ILogger<StartupService> logger, PlatformInfoProvider platformInfoProvider,IPiUpsHatService piUpsHatService)
        {
            _gpioService = gpioService;
            _adcChannelReaderService = adcChannelReaderService;
            _logger = logger;
            this.platformInfoProvider = platformInfoProvider;
            _rigOptions = options.Value;
            _piUpsHatService = piUpsHatService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!File.Exists("/dev/i2c-0") && !platformInfoProvider.IsWindows)
            {
                throw new InvalidOperationException("I2C is not enabled. If this is a Raspberry Pi: sudo raspi-config, Interface Options, I2C, Enable");
            }

            await _gpioService.SetGpio(_rigOptions.RXTX_CHANGEOVER_RELAY_PIN, true);
            await _gpioService.SetGpio(_rigOptions.PA_RELAY_PIN, true);

            _timer = new Timer(Tick, null, 0, 10000);
        }

        private async void Tick(object? state)
        {
            try
            {
                var reading0 = await _adcChannelReaderService.Read(0, 0);
                var reading1 = await _adcChannelReaderService.Read(0, 1);
                var reading2 = await _adcChannelReaderService.Read(0, 2);
                var reading3 = await _adcChannelReaderService.Read(0, 3);

                _logger.LogInformation($"ADC: {reading0.Millivolts / 1000.0:0.000}V {reading1.Millivolts / 1000.0:0.000}V {reading2.Millivolts / 1000.0:0.000}V {reading3.Millivolts / 1000.0:0.000}V");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading ADC - this will be suspended");
                await StopAsync(CancellationToken.None);
            }

            try
            {
                var UPSreading0 = await _piUpsHatService.Read();
              

                _logger.LogInformation($"UPS Battery: {UPSreading0.BusVoltage:0.000}V");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading UPS - this will be suspended");
                await StopAsync(CancellationToken.None);
            }
        }

       

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }
    }
}