namespace rig_controller.Services
{
    public class RigStateService
    {
        public RigState RigState { get; private set; } = new();
    }

    public class RigState
    {
        public long Frequency { get; set; }
    }
}
