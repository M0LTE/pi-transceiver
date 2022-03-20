using rig_controller.Services;

internal class NullGpioService : IGpioService
{
    public void Dispose() { }
    public Task SetGpio(int pin, bool state) => Task.CompletedTask;
}