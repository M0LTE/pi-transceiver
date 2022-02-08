using System.Device.Gpio;

namespace rig_controller.Services
{
    public class GpioService
    {
        private readonly ILogger<GpioService> _logger;
        private static bool pigsWontFly;

        public GpioService (ILogger<GpioService> logger)
        {
            _logger = logger;
        }
        
        public Task SetGpio(int pin, bool state)
        {
            if (pigsWontFly)
            {
                return Task.CompletedTask;
            }

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                _logger.LogTrace($"No GPIO on Windows ({pin}:{(state ? 1 : 0)}");
                return Task.CompletedTask;
            }

            // HACK - replace with https://abyz.me.uk/rpi/pigpio/sif.html#cmdCmd_t
            try
            {
                using var controller = new GpioController();
                controller.OpenPin(pin, PinMode.Output);
                controller.Write (pin, state);


                //System.Diagnostics.Process.Start("pigs", $"w {pin} {(state ? 1 : 0)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling pigs, won't try that any more (all GPIO setting is disabled from now on)");
                pigsWontFly = true;
            }

            return Task.CompletedTask;
        }
    }
}
