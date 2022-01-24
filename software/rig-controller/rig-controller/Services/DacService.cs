using Unosquare.RaspberryIO;


namespace rig_controller.Services
{
    
    public class i2cDacService 
    {

        private readonly ILogger<i2cDacService> _logger;

        public i2cDacService(ILogger<i2cDacService> logger)
        {
            _logger = logger;

            Pi.Init<Unosquare.WiringPi.BootstrapWiringPi>();
        }

        public Task SetDAC(int device, out int value, int voltage)
        {
            //double scale = await GetScale(device, channel);

            var myDevice =  Pi.I2C.AddDevice(0x62);

            value = myDevice.DeviceId;

            myDevice.Write(Convert.ToByte(0x40));
            myDevice.Write(Convert.ToByte(voltage / 16));
            myDevice.Write(Convert.ToByte((voltage % 16) << 4));

            



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
