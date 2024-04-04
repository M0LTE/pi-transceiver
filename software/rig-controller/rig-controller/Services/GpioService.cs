using System.Device.Gpio;

namespace rig_controller.Services
{
    public class GpioService
    {
<<<<<<< Updated upstream
        private readonly ILogger<GpioService> _logger;
=======
        Task SetGpio(int pin, bool state);
    }

    public class NativeGpioService : IGpioService
    {
        private readonly List<int> openedPins = new();
        private readonly GpioController controller = new();

        public void Dispose() => controller.Dispose();

        public Task SetGpio(int pin, bool state)
        {
            try
            {
            if (!openedPins.Contains(pin))
            {
                controller.OpenPin(pin, PinMode.Output);
                openedPins.Add(pin);
            }

            controller.Write(pin, state ? PinValue.High : PinValue.Low);

            }
            catch
            {}

            return Task.CompletedTask;
        }
    }

    public class PigsGpioService : IGpioService
    {
        private readonly ILogger<PigsGpioService> _logger;
>>>>>>> Stashed changes
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
                using var controller = new GpioController( );
                controller.OpenPin(pin, PinMode.Output);
                controller.Write (pin, state ? PinValue.High : PinValue.Low);

                _logger.LogTrace($"Set GPIO ({pin}:{(state ? 1 : 0)}");

             

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
