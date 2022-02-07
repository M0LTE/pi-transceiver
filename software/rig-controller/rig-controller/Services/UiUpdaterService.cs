using Microsoft.AspNetCore.SignalR;
using rig_controller.Hubs;

namespace rig_controller.Services
{
    public class UiUpdaterService
    {
        private readonly IHubContext<UiHub> uiHubContext;
        private readonly ILogger<UiUpdaterService> logger;
        private readonly RigStateService rigStateService;
        private readonly i2cDacService dacService;
        private readonly UPSHatService upshatService;

        public UiUpdaterService(IHubContext<UiHub> uiHubContext, ILogger<UiUpdaterService> logger, RigStateService rigStateService, i2cDacService dacService, UPSHatService upshatService)
        {
            this.uiHubContext = uiHubContext;
            this.logger = logger;
            this.rigStateService = rigStateService;
            this.dacService = dacService;
            this.upshatService = upshatService;
        }

        public async Task SetFrequency()
        {
            var f = rigStateService.RigState.Frequency;
            var v = 0;

            INA219_Reading reading;

            //await upshatService.SetAddress(0x42);
            //await upshatService.set_calibration_32V_2A();

            string digits = (f / 1000000.0).ToString("0000.000");

            logger.LogInformation($"Server setting frequency in UI to {digits}");

            await uiHubContext.Clients.All.SendAsync("SetFrequency", digits[0], digits[1], digits[2], digits[3], digits[5], digits[6], digits[7]);

            await AddLogLine("Server told UI to set frequency to " + f / 1000000.0);

            //test
            await dacService.SetDAC(i2cDacService.MCP4726_DEFAULT_ADDRESS, false,Convert.ToUInt16( f / 1000000.0),out v);

            await AddLogLine("DeviceId " + v);

            reading = await upshatService.Read();

            await AddLogLine("Battery " + reading.Percent );
            await AddLogLine("Bus V " + reading.Bus_voltage);


        }

        public async Task AddLogLine(string message)
        {
            await uiHubContext.Clients.All.SendAsync("AddLogLine", message);
        }
    }
}
