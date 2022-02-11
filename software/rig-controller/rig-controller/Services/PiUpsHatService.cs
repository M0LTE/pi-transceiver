using Iot.Device.Adc;
using System.Device.I2c;
using UnitsNet;


namespace rig_controller.Services
{
    public interface IPiUpsHatService : IDisposable
    {
        Task<Ina219Reading> Read();
    }

    public class PiUpsHatService : IPiUpsHatService
    {
        private readonly Ina219? device;
        private readonly ushort calValue = 4096;
        private readonly float currentLsb = 0.0001f;  // Current LSB = 100uA per bit
        private readonly PlatformInfoProvider platformInfoProvider;

        public PiUpsHatService(PlatformInfoProvider platformInfoProvider)
        {
            this.platformInfoProvider = platformInfoProvider;

            if (!platformInfoProvider.IsWindows)
            {
                I2cConnectionSettings settings = new(1, 0x42);
                device = new Ina219(settings);
                SetupDevice();
            }
        }

        private void SetupDevice()
        {
            SetCalibration();
            device.ShuntAdcResolutionOrSamples = Ina219AdcResolutionOrSamples.Adc32Sample;
            device.BusAdcResolutionOrSamples = Ina219AdcResolutionOrSamples.Adc32Sample;
            device.BusVoltageRange = Ina219BusVoltageRange.Range32v;
            device.PgaSensitivity = Ina219PgaSensitivity.PlusOrMinus320mv;
            device.OperatingMode = Ina219OperatingMode.ShuntAndBusContinuous;
        }

        public Task<Ina219Reading> Read()
        {
            if (platformInfoProvider.IsWindows)
            {
                return Task.FromResult(new Ina219Reading { BusVoltage = new(), ShuntVoltage = new(), Milliamps = default, Watts = default, Percent = default, OnBattery = default });
            }

            SetCalibration();
            var shuntVoltage = device.ReadShuntVoltage();

            SetCalibration();
            var busVoltage = device.ReadBusVoltage();

            SetCalibration();
            var milliAmps = device.ReadCurrent().Value;

            SetCalibration();
            var watts = (double)(device.ReadPower().Watts);
            var percent = (float)((busVoltage.Value - 6) / 2.4 * 100.0);

            return Task.FromResult(new Ina219Reading { BusVoltage = busVoltage, ShuntVoltage = shuntVoltage, Milliamps = milliAmps, Watts = watts, Percent = percent, OnBattery = milliAmps < 0 });
        }

        private void SetCalibration() => device.SetCalibration(calValue, currentLsb);

        public void Dispose()
        {
            device?.Dispose();
        }
    }

    public record Ina219Reading
    {
        public ElectricPotential BusVoltage { get; set; }
        public ElectricPotential ShuntVoltage { get; set; }
        public double Milliamps { get; set; }
        public double Watts { get; set; }
        public float Percent { get; set; }
        public bool OnBattery { get; set; }
    }
}
