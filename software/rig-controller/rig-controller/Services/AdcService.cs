using System.Collections.Concurrent;

namespace rig_controller.Services
{
    public interface IAdcChannelReaderService
    {
        Task<AdcReading> Read(int device, int channel);
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