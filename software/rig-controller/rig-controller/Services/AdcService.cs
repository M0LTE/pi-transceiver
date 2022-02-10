using Iot.Device.Ads1115;
using System.Collections.Concurrent;
using System.Device.I2c;

namespace rig_controller.Services
{
    public interface IAdcChannelReaderService : IDisposable
    {
        Task<AdcReading> Read(int device, int channel);
    }

    public class Ads1115NativeChannelReaderService : IAdcChannelReaderService
    {
        // System.IO.IOException: Error 2. Can not open I2C device file '/dev/i2c-0'.
        //   - sudo raspi-config, Interface Options, I2C, Enable

        private const int BUS_1 = 1;

        private readonly Dictionary<int, int> addressMap = new() {
            { 0, 0x48 },
            { 1, 0x49 },
            { 2, 0x4a },
            { 3, 0x4b },
        };

        private readonly Dictionary<int, InputMultiplexer> channelMap = new()
        {
            { 0, InputMultiplexer.AIN0 },
            { 1, InputMultiplexer.AIN1 },
            { 2, InputMultiplexer.AIN2 },
            { 3, InputMultiplexer.AIN3 },
        };

        private readonly Dictionary<int,Ads1115> devices;

        public Ads1115NativeChannelReaderService()
        {
            devices = new Dictionary<int, Ads1115>();
        }

        public Task<AdcReading> Read(int deviceId, int channel)
        {
            if (!devices.ContainsKey(deviceId))
            {
                var i2cDevice = I2cDevice.Create(new I2cConnectionSettings(BUS_1, addressMap[deviceId]));

                devices.Add(deviceId, new Ads1115(i2cDevice));
            }

            var device = devices[deviceId];

            var reading = device.ReadVoltage(channelMap[channel]);

            return Task.FromResult(new AdcReading { Channel = channel, Device = deviceId, Millivolts = (int)reading.Millivolts });
        }

        public void Dispose()
        {
            foreach (var device in devices.Values)
            {
                device.Dispose();
            }
        }
    }

    /// <summary>
    /// An implementation which reads channels of the ADS1115 ADC via parsing /sys/bus/iio/devices/iio:devicen/in_voltagen_raw
    /// This requires the following in /boot/config.txt:
    /// dtparam=i2c_arm=on
    /// dtoverlay=ads1115
    /// dtparam=cha_enable
    /// dtparam=chb_enable
    /// dtparam=chc_enable
    /// dtparam=chd_enable
    /// </summary>
    public class ADS1115SysBusAdcChannelReaderService : IAdcChannelReaderService
    {
        private static readonly ConcurrentDictionary<DeviceChannel, double> scalesCache = new();

        public async Task<AdcReading> Read(int device, int channel)
        {
            double scale = await GetScale(device, channel);

            if (!int.TryParse((await File.ReadAllTextAsync($"/sys/bus/iio/devices/iio:device{device}/in_voltage{channel}_raw")).Trim(), out int raw))
            {
                throw new NotImplementedException();
            }

            var millivolts = (int)(raw * scale);

            return new AdcReading { Channel = channel, Device = device, Millivolts = millivolts };
        }

        private static async Task<double> GetScale(int device, int channel)
        {
            var dc = new DeviceChannel { Device = device, Channel = channel };

            double scale;
            if (scalesCache.ContainsKey(dc))
            {
                scale = scalesCache[dc];
            }
            else
            {
                scale = await ReadScaleFromSystem(dc);
                scalesCache.TryAdd(dc, scale);
            }

            return scale;
        }

        private static async Task<double> ReadScaleFromSystem(DeviceChannel dc)
        {
            if (!double.TryParse((await File.ReadAllTextAsync($"/sys/bus/iio/devices/iio:device{dc.Device}/in_voltage{dc.Channel}_scale")).Trim(), out var scale))
            {
                return Double.NaN;
            }

            return scale;
        }

        public void Dispose()
        {
        }

        private record DeviceChannel
        {
            public int Device { get; set; }
            public int Channel { get; set; }
        }
    }

    public record AdcReading
    {
        public int Device { get; set; }
        public int Channel { get; set; }
        public int Millivolts { get; set; }
    }
}