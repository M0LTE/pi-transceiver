using System.Collections.Concurrent;
using Iot.Device.Ads1115;
using System.Device.I2c;


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
        public Ads1115 dev;
        private static readonly ConcurrentDictionary<DeviceChannel, double> scalesCache = new();

        private void InitializeSystem()
        {



            //I2cConnectionSettings settings = new(1, (int)I2cAddress.GND);
            //I2cDevice device = I2cDevice.Create(settings);


            //if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            //{

            //    dev = new Iot.Device.Ads1115.Ads1115(device, InputMultiplexer.AIN0, MeasuringRange.FS4096);
            //}

        }

        public async Task<AdcReading> Read(int device, int channel)

        {
            var millivolts = 0;

            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {

                I2cConnectionSettings settings = new(1, (int)I2cAddress.GND);
                I2cDevice adcdevice = I2cDevice.Create(settings);
                dev = new Iot.Device.Ads1115.Ads1115(adcdevice, InputMultiplexer.AIN0, MeasuringRange.FS4096);
                millivolts = (int) dev.ReadVoltage(InputMultiplexer.AIN0).Millivolts;

                //double scale = await GetScale(device, channel);

                //if (!int.TryParse((await File.ReadAllTextAsync($"/sys/bus/iio/devices/iio:device{device}/in_voltage{channel}_raw")).Trim(), out int raw))
                //{
                //    throw new NotImplementedException();
                //}

                //millivolts = (int)(raw * scale);
            }
           

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