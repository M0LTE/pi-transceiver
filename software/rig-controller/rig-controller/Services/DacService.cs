using Unosquare.RaspberryIO;


namespace rig_controller.Services
{
    
    public class i2cDacService 
    {
        const byte MCP4726_CMD_WRITEDAC = 0x40; // Writes data to the DAC
        const byte MCP4726_CMD_WRITEDACEEPROM = 0x60; // Writes data to the DAC and the EEPROM (persisting the assigned value after reset)

        private readonly ILogger<i2cDacService> _logger;

        public i2cDacService(ILogger<i2cDacService> logger)
        {
            _logger = logger;

            Pi.Init<Unosquare.WiringPi.BootstrapWiringPi>();
        }

        public Task SetDAC(int device, out int value, bool writeEEPROM,UInt16 voltage)
        {
            //double scale = await GetScale(device, channel);

            var myDevice =  Pi.I2C.AddDevice(0x62);

            value = myDevice.DeviceId;

            byte e = writeEEPROM ? MCP4726_CMD_WRITEDACEEPROM : MCP4726_CMD_WRITEDAC;

            byte o0 = (byte)(voltage >> 4); // Another way
            byte o1 = (byte)((voltage & 15) << 4);


            myDevice.Write(e);
            myDevice.Write(o0);
            myDevice.Write(o1);

            



            //foreach (var i2cdevice in Pi.I2C.Devices)
            //{
            //    _logger.LogTrace($"Registered I2C Device: {i2cdevice.DeviceId}");
               
            //}

            //if (!int.TryParse((await File.ReadAllTextAsync($"/sys/bus/iio/devices/iio:device{device}/in_voltage{channel}_raw")).Trim(), out int raw))
            //{
            //    throw new NotImplementedException();
            //}

            //var millivolts = (int)(raw * scale);

            return Task.CompletedTask;
        }
    }

 
}
