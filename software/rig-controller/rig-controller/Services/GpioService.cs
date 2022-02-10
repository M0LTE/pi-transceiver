using System.Device.Gpio;

namespace rig_controller.Services
{
    public interface IGpioService : IDisposable
    {
        Task SetGpio(int pin, bool state);
    }

    public class NativeGpioService : IGpioService
    {
        private readonly List<int> openedPins = new();
        private readonly GpioController controller = new();

        public void Dispose() => controller.Dispose();

        public Task SetGpio(int pin, bool state)
        {
            if (!openedPins.Contains(pin))
            {
                controller.OpenPin(pin, PinMode.Output);
                openedPins.Add(pin);
            }

            controller.Write(pin, state ? PinValue.High : PinValue.Low);

            return Task.CompletedTask;
        }
    }

    public class PigsGpioService : IGpioService
    {
        private readonly ILogger<PigsGpioService> _logger;
        private static bool pigsWontFly;

        public PigsGpioService(ILogger<PigsGpioService> logger)
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
                //System.Diagnostics.Process.Start("pigs", $"w {pin} {(state ? 1 : 0)}");
                using var controller = new GpioController();
                controller.OpenPin(pin, PinMode.Output);
                controller.Write(pin, state ? PinValue.High : PinValue.Low);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling pigs, won't try that any more (all GPIO setting is disabled from now on)");
                pigsWontFly = true;
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
