namespace rig_controller.Services
{
    public class FlexKnobHostedService : IHostedService
    {
        private readonly FlexKnob flexKnob = new();
        private readonly RigStateService rigStateService;
        private readonly UiUpdaterService uiUpdaterService;

        public FlexKnobHostedService(RigStateService rigStateService, UiUpdaterService uiUpdaterService)
        {
            this.rigStateService = rigStateService;
            this.uiUpdaterService = uiUpdaterService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            flexKnob.OnButtonPush += ButtonPush;
            flexKnob.OnRotate += Rotate;
            Task.Run(() => flexKnob.Run(), cancellationToken);
            return Task.CompletedTask;
        }

        private void Rotate(Direction direction, int speed)
        {
            if (direction == Direction.Up && rigStateService.RigState.Frequency >= 3000000000)
            {
                return;
            }
            else if (direction == Direction.Down && rigStateService.RigState.Frequency <= 1000)
            {
                return;
            }

            rigStateService.RigState.Frequency += (direction == Direction.Up ? 1000 : -1000) * speed;
            Task.Run(async () => await uiUpdaterService.SetFrequency());
        }

        private void ButtonPush(Button arg1, TapType arg2)
        {
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}