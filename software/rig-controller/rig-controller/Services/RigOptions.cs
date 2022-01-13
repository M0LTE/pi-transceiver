namespace rig_controller.Services
{
    public class RigOptions
    {
        /// <summary>
        /// Which GPIO pin the PA bias relay is connected to
        /// </summary>
        public int PA_RELAY_PIN { get; set; } = 26;

        /// <summary>
        /// The time it takes for the PA bias relay to complete switching
        /// </summary>
        public int PA_RELAY_DELAY { get; set; } = 100;

        /// <summary>
        /// Which GPIO pin the RX/TX transover relay is connected to
        /// </summary>
        public int RXTX_CHANGEOVER_RELAY_PIN { get; set; } = 13;

        /// <summary>
        /// The time it takes for the RX/TX changeover relay to complete switching
        /// </summary>
        public int RXTX_RELAY_DELAY { get; set; } = 100;

        /// <summary>
        /// The time it takes from muting the flowgraph to the output being zero-valued. Needs measuring.
        /// </summary>
        public int FLOWGRAPH_DELAY { get; set; } = 500;
    }
}