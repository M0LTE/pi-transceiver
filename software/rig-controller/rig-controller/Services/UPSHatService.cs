
using System.Runtime.InteropServices;
using Iot.Device.Adc;


namespace rig_controller.Services
{
    public class UPSHatService
    {

        private static int OPEN_READ_WRITE = 2;
        private static int I2C_CLIENT = 0x0703;

        public Ina219 dev;




        private ushort cal_value = 0;
        private float current_lsb = 0;
        private double power_lsb = 0;



        private byte deviceaddress;

        public UPSHatService()
        {
            System.Device.I2c.I2cConnectionSettings settings = new System.Device.I2c.I2cConnectionSettings(1, 0x42);


            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                dev = new Ina219(settings);
                set_calibration_32V_2A();

            }

        }

        public Task SetAddress(byte deviceAddress)
        {
            deviceaddress = deviceAddress;


            return Task.CompletedTask;
        }





        public Task set_calibration_32V_2A()
        {
            current_lsb = (float).0001;  // Current LSB = 100uA per bit

            cal_value = 4096;

            power_lsb = (double).002;  // Power LSB = 2mW per bit


            dev.SetCalibration(cal_value, current_lsb);



            dev.ShuntAdcResolutionOrSamples = Ina219AdcResolutionOrSamples.Adc32Sample;
            dev.BusAdcResolutionOrSamples = Ina219AdcResolutionOrSamples.Adc32Sample;
            dev.BusVoltageRange = Ina219BusVoltageRange.Range32v;
            dev.PgaSensitivity = Ina219PgaSensitivity.PlusOrMinus320mv;
            dev.OperatingMode = Ina219OperatingMode.ShuntAndBusContinuous;



            return Task.CompletedTask;

        }

        public async Task<INA219_Reading> Read()
        {

            UnitsNet.ElectricPotential shunt_voltage;
            UnitsNet.ElectricPotential bus_voltage;
            double current_ma;
            double power_w;
            float percent;
            bool on_battery;





            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {


                dev.SetCalibration(cal_value, current_lsb);
                shunt_voltage = dev.ReadShuntVoltage();



                dev.SetCalibration(cal_value, current_lsb);
                bus_voltage = dev.ReadBusVoltage();


                dev.SetCalibration(cal_value, current_lsb);
                current_ma = dev.ReadCurrent().Value;


                dev.SetCalibration(cal_value, current_lsb);

                power_w = (double)(dev.ReadPower().Watts);

                percent = (float)((bus_voltage.Value - 6) / 2.4 * 100);

                on_battery = (current_ma < 0);


            }
            else
            {
                bus_voltage = new UnitsNet.ElectricPotential();
                shunt_voltage = new UnitsNet.ElectricPotential();
                current_ma = 0;
                power_w = 0;
                percent = 0;
                on_battery = false;
            }

            return new INA219_Reading { Bus_voltage = bus_voltage, Shunt_voltage = shunt_voltage, Current_ma = current_ma, Power_w = power_w, Percent = percent, On_battery = on_battery };
        }






    }

    public record INA219_Reading
    {
        public UnitsNet.ElectricPotential Bus_voltage { get; set; }
        public UnitsNet.ElectricPotential Shunt_voltage { get; set; }
        public double Current_ma { get; set; }

        public double Power_w { get; set; }

        public float Percent { get; set; }

        public bool On_battery { get; set; }
    }
}
