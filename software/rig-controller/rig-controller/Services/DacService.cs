using Unosquare.RaspberryIO;

namespace rig_controller.Services
{
    public interface IDacChannelWriterService
    {
        Task<AdcWriting> Write(int device, int value);
    }
    public class DacService : IDacChannelWriterService
    {
       
        public async Task<AdcWriting> Write(int device, int value)
        {
            //double scale = await GetScale(device, channel);

            var myDevice = Pi.I2C.AddDevice(0x63);

            foreach (var i2cdevice in Pi.I2C.Devices)
            {
                Console.WriteLine($"Registered I2C Device: {i2cdevice.DeviceId}");
            }

            //if (!int.TryParse((await File.ReadAllTextAsync($"/sys/bus/iio/devices/iio:device{device}/in_voltage{channel}_raw")).Trim(), out int raw))
            //{
            //    throw new NotImplementedException();
            //}

            //var millivolts = (int)(raw * scale);

            return new AdcWriting { Device = device, Value = value };
        }
    }

    public record AdcWriting
    {
        public int Device { get; set; }
        public int Value { get; set; }
    }
}
