namespace rig_controller.Services
{
    public class FlowgraphControlService
    {
        private readonly ILogger<FlowgraphControlService> logger;

        public FlowgraphControlService(ILogger<FlowgraphControlService> logger)
        {
            this.logger = logger;
        }

        internal Task MuteAudioSource()
        {
            logger.LogTrace("TODO: mute flow graph");
            return Task.CompletedTask;
        }

        internal Task UnmuteAudioSource()
        {
            logger.LogTrace("TODO: unmute flow graph");
            return Task.CompletedTask;
        }
    }
}
