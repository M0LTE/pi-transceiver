namespace rig_controller.Services
{
    public class GpioService
    {
        // HACK - replace with https://abyz.me.uk/rpi/pigpio/sif.html#cmdCmd_t
        public async Task SetGpio(int pin, bool state) => System.Diagnostics.Process.Start("pigs", $"w {pin} {(state ? 1 : 0)}");
    }
}
