using CookComputing.XmlRpc;

namespace rig_controller.Services
{
    public class FlowgraphControlService
    {
        private readonly RigStateService rigStateService;

        private readonly ILogger<FlowgraphControlService> logger;

        [XmlRpcUrl("http://localhost:8080")]
        public interface ISet_freq_value : IXmlRpcProxy
        {
            [XmlRpcMethod("set_freq_value")]
            string set_freq_value(long test);
        }

        readonly ISet_freq_value proxy = XmlRpcProxyGen.Create<ISet_freq_value>();

        public FlowgraphControlService(ILogger<FlowgraphControlService> logger, RigStateService rigStateService)
        {
            this.logger = logger;
            this.rigStateService = rigStateService;
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

        internal Task SetFrequency()
        {
           
            try
            { string ret = proxy.set_freq_value(rigStateService.RigState.Frequency); }
            catch
            {
                logger.LogTrace("XMLRPC Error");
            }
           


            return Task.CompletedTask;
        }
    }
}
