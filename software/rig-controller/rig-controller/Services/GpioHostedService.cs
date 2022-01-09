namespace rig_controller.Services
{
    public class GpioHostedService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(RelayLoop, cancellationToken);

            return Task.CompletedTask;
        }

        private void RelayLoop()
        {
            while (true)
            {
                Set(13, true);
                Delay(500);
                Set(26, true);
                Delay(500);
                Set(13, false);
                Delay(500);
                Set(26, false);
                Delay(500);
            }

            static void Set(int pin, bool state) => System.Diagnostics.Process.Start("pigs", $"w {pin} {(state ? 1 : 0)}");
            static void Delay(int ms) => Thread.Sleep(ms);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
