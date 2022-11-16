using Microsoft.Extensions.Options;

namespace rig_controller.Services
{
    public class StartupService : IHostedService
    {
        private readonly IGpioService _gpioService;
        private readonly IAdcChannelReaderService _adcChannelReaderService;
        private readonly IPiUpsHatService  _piUpsHatService;
        private readonly II2cDacService _i2cDacService;
        private readonly IFanService _fanService;
        private readonly ILogger<StartupService> _logger;
        private readonly PlatformInfoProvider platformInfoProvider;
        private readonly RigOptions _rigOptions;
        private Timer? _timer;

        public StartupService(IGpioService gpioService, IOptions<RigOptions> options, IAdcChannelReaderService adcChannelReaderService,
            ILogger<StartupService> logger, PlatformInfoProvider platformInfoProvider,IPiUpsHatService piUpsHatService, II2cDacService i2CDacService, IFanService fanService)
        {
            _gpioService = gpioService;
            _adcChannelReaderService = adcChannelReaderService;
            _logger = logger;
            this.platformInfoProvider = platformInfoProvider;
            _rigOptions = options.Value;
            _piUpsHatService = piUpsHatService;
            _i2cDacService = i2CDacService;
            _fanService = fanService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!File.Exists("/dev/i2c-1") && !platformInfoProvider.IsWindows)
            {
                throw new InvalidOperationException("I2C is not enabled. If this is a Raspberry Pi: sudo raspi-config, Interface Options, I2C, Enable");
            }

            await _gpioService.SetGpio(_rigOptions.RXTX_CHANGEOVER_RELAY_PIN, true);
            await _gpioService.SetGpio(_rigOptions.PA_RELAY_PIN, true);

            _timer = new Timer(Tick, null, 0, 10000);

            await _fanService.PWMFanSimpleExample();
        }

        private async void Tick(object? state)
        {
            try
            {
                if (_adcChannelReaderService.ENABLED)
                {
                    var reading0 = await _adcChannelReaderService.Read(0, 0);
                    var reading1 = await _adcChannelReaderService.Read(0, 1);
                    var reading2 = await _adcChannelReaderService.Read(0, 2);
                    var reading3 = await _adcChannelReaderService.Read(0, 3);

                    _logger.LogInformation($"ADC: {reading0.Millivolts / 1000.0:0.000}V {reading1.Millivolts / 1000.0:0.000}V {reading2.Millivolts / 1000.0:0.000}V {reading3.Millivolts / 1000.0:0.000}V");

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading ADC - this will be suspended");
                _adcChannelReaderService.ENABLED = false;
                //await StopAsync(CancellationToken.None);
            }

            try
            {
                var UPSreading0 = await _piUpsHatService.Read();
              

                //_logger.LogInformation($"UPS Battery: {UPSreading0.BusVoltage:0.000}V");
                //_logger.LogInformation($"UPS Milliamps: {UPSreading0.Milliamps:0.000}V");
                //_logger.LogInformation($"UPS Power: {UPSreading0.Milliamps:0.000}V");

                _logger.LogInformation($"UPS: {UPSreading0.BusVoltage :0.000}V {UPSreading0.Milliamps :0.000}mA {UPSreading0.Watts :0.000}W {UPSreading0.Percent / 1000.0:0.000}% {UPSreading0.OnBattery:0}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading UPS - this will be suspended");
                //await StopAsync(CancellationToken.None);
            }

            try
            {
                Random rnd = new Random();
                int iout;

                await _i2cDacService.SetDAC(0x60, false, (ushort)rnd.Next(4095), out iout);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing DAC - this will be suspended");
            }

        }

       

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }
    }
}