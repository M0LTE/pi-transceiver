using CookComputing.XmlRpc;

namespace rig_controller.Services
{
    public interface IFlowgraphControlService
    {
        Task<bool> MuteAudioSource();
        Task<bool> SetFrequency(long hz);
        Task<bool> UnmuteAudioSource();
    }

    public class GnuRadioFlowgraphControlService : IFlowgraphControlService
    {
        private readonly RigStateService rigStateService;
        private readonly ILogger<GnuRadioFlowgraphControlService> logger;
        private static readonly IFlowgraphXmlRpcProxy proxy = XmlRpcProxyGen.Create<IFlowgraphXmlRpcProxy>();

        public GnuRadioFlowgraphControlService(ILogger<GnuRadioFlowgraphControlService> logger, RigStateService rigStateService)
        {
            this.logger = logger;
            this.rigStateService = rigStateService;
        }

        public Task<bool> MuteAudioSource()
        {
            logger.LogTrace("TODO: mute flow graph");
            return Task.FromResult(true);
        }

        public Task<bool> UnmuteAudioSource()
        {
            logger.LogTrace("TODO: unmute flow graph");
            return Task.FromResult(true);
        }

        public Task<bool> SetFrequency(long hz)
        {
            try
            {
                proxy.SetFrequency(hz);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "XMLRPC error");
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }

    [XmlRpcUrl("http://localhost:8080")]
    public interface IFlowgraphXmlRpcProxy : IXmlRpcProxy
    {
        [XmlRpcMethod("set_freq_value")]
        string SetFrequency(long hz);
    }
}