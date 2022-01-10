namespace rig_controller.Services
{
    public class GpioService
    {
        private readonly ILogger<GpioService> logger;

        public GpioService (ILogger<GpioService> logger)
        {
            this.logger = logger;
        }
        
        public Task SetGpio(int pin, bool state)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                logger.LogTrace($"No GPIO on Windows ({pin}:{(state ? 1 : 0)}");
                return Task.CompletedTask;
            }

            // HACK - replace with https://abyz.me.uk/rpi/pigpio/sif.html#cmdCmd_t
            System.Diagnostics.Process.Start("pigs", $"w {pin} {(state ? 1 : 0)}");

            return Task.CompletedTask;
        }
    }
}
