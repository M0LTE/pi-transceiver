using Iot.Device.Adc;
using Iot.Device.Bno055;
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
            try{
            SetCalibration();
            device.ShuntAdcResolutionOrSamples = Ina219AdcResolutionOrSamples.Adc32Sample;
            device.BusAdcResolutionOrSamples = Ina219AdcResolutionOrSamples.Adc32Sample;
            device.BusVoltageRange = Ina219BusVoltageRange.Range32v;
            device.PgaSensitivity = Ina219PgaSensitivity.PlusOrMinus320mv;
            device.OperatingMode = Ina219OperatingMode.ShuntAndBusContinuous;
            }
            catch
            {}
        }

        public Task<Ina219Reading> Read()
        {
            ElectricPotential shuntVoltage = ElectricPotential.Zero;
            ElectricPotential busVoltage = ElectricPotential.Zero;
            Double milliAmps = 0;
            Double watts = 0;
            float percent = 0;

            if (platformInfoProvider.IsWindows)
            {
                return Task.FromResult(new Ina219Reading { BusVoltage = new(), ShuntVoltage = new(), Milliamps = default, Watts = default, Percent = default, OnBattery = default });
            }

            try
            {
            SetCalibration();
            shuntVoltage = device.ReadShuntVoltage();

            SetCalibration();
            busVoltage = device.ReadBusVoltage();

            SetCalibration();
            milliAmps = device.ReadCurrent().Value;


            SetCalibration();
            watts = (double)(device.ReadPower().Watts);
            percent = (float)((busVoltage.Value - 6) / 2.4 * 100.0);

            }
        catch
            {}

            return Task.FromResult(new Ina219Reading { BusVoltage = busVoltage, ShuntVoltage = shuntVoltage, Milliamps = milliAmps, Watts = watts, Percent = percent, OnBattery = milliAmps < 0 });
        }

        private void SetCalibration() 
        {
            try
            {         
                   device.SetCalibration(calValue, currentLsb);
            }
            catch
            {}

        }

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
