
using System.Runtime.InteropServices;
using Iot.Device.Adc;


namespace rig_controller.Services
{
    public class UPSHatService
    {

        private static int OPEN_READ_WRITE = 2;
        private static int I2C_CLIENT = 0x0703;

        public Ina219 dev;


        // Config Register (R/W)
        const byte REG_CONFIG = 0x00;
        // SHUNT VOLTAGE REGISTER (R)
        const byte REG_SHUNTVOLTAGE = 0x01;

        // BUS VOLTAGE REGISTER (R)
        const byte REG_BUSVOLTAGE = 0x02;

        // POWER REGISTER (R)
        const byte REG_POWER = 0x03;

        // CURRENT REGISTER (R)
        const byte REG_CURRENT = 0x04;

        // CALIBRATION REGISTER (R/W)
        const byte REG_CALIBRATION = 0x05;


        const byte BUSV_RANGE_16V = 0x00;      // set bus voltage range to 16V
        const byte BUSV_RANGE_32V = 0x01;      // set bus voltage range to 32V (default)

        const byte GAIN_DIV_1_40MV = 0x00;      // shunt prog. gain set to  1, 40 mV range
        const byte GAIN_DIV_2_80MV = 0x01;      // shunt prog. gain set to /2, 80 mV range
        const byte GAIN_DIV_4_160MV = 0x02;      // shunt prog. gain set to /4, 160 mV range
        const byte GAIN_DIV_8_320MV = 0x03;      // shunt prog. gain set to /8, 320 mV range

        const byte ADCRES_9BIT_1S = 0x00;      //  9bit,   1 sample,     84us
        const byte ADCRES_10BIT_1S = 0x01;     // 10bit,   1 sample,    148us
        const byte ADCRES_11BIT_1S = 0x02;     // 11 bit,  1 sample,    276us
        const byte ADCRES_12BIT_1S = 0x03;      // 12 bit,  1 sample,    532us
        const byte ADCRES_12BIT_2S = 0x09;      // 12 bit,  2 samples,  1.06ms
        const byte ADCRES_12BIT_4S = 0x0A;      // 12 bit,  4 samples,  2.13ms
        const byte ADCRES_12BIT_8S = 0x0B;      // 12bit,   8 samples,  4.26ms
        const byte ADCRES_12BIT_16S = 0x0C;    // 12bit,  16 samples,  8.51ms
        const byte ADCRES_12BIT_32S = 0x0D;      // 12bit,  32 samples, 17.02ms
        const byte ADCRES_12BIT_64S = 0x0E;     // 12bit,  64 samples, 34.05ms
        const byte ADCRES_12BIT_128S = 0x0F;    // 12bit, 128 samples, 68.10ms

        const byte MODE_POWERDOW = 0x00;      // power down
        const byte MODE_SVOLT_TRIGGERED = 0x01;      // shunt voltage triggered
        const byte MODE_BVOLT_TRIGGERED = 0x02;      // bus voltage triggered
        const byte MODE_SANDBVOLT_TRIGGERED = 0x03;      // shunt and bus voltage triggered
        const byte MODE_ADCOFF = 0x04;      // ADC off
        const byte MODE_SVOLT_CONTINUOUS = 0x05;      // shunt voltage continuous
        const byte MODE_BVOLT_CONTINUOUS = 0x06;      // bus voltage continuous
        const byte MODE_SANDBVOLT_CONTINUOUS = 0x07;      // shunt and bus voltage continuous



        private ushort cal_value = 0;
        private float current_lsb = 0;
        private double power_lsb = 0;

        private byte bus_voltage_range;
        private byte gain;
        private byte bus_adc_resolution;
        private byte shunt_adc_resolution;
        private byte mode;
        private int config;


        [DllImport("libc.so.6", EntryPoint = "open")]
        private static extern int Open(string fileName, int mode);

        [DllImport("libc.so.6", EntryPoint = "ioctl", SetLastError = true)]
        private static extern int Ioctl(int fd, int request, int data);

        [DllImport("libc.so.6", EntryPoint = "read", SetLastError = true)]
        private static extern int Read(int handle, byte[] data, int length);

        [DllImport("libc.so.6", EntryPoint = "write", SetLastError = true)]
        private static extern int Write(int handle, byte[] data, int length);

        private byte  deviceaddress;

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

        public Task INA219_Write(byte register, int data)
        {
            byte[] temp = new byte[3];

            temp[0] = register;
            temp[2] = (byte)(data & 0xFF);
            temp[1] = (byte)((data & 0xFF00) >> 8);

            int i2cHandle = Open("/dev/i2c-1", OPEN_READ_WRITE);
            // mount the device at address 'device' for communication
            //int registerAddress = 0x62;
            int deviceReturnCode = Ioctl(i2cHandle, I2C_CLIENT, deviceaddress);

            deviceReturnCode = Write(i2cHandle, temp, 3);

            return Task.FromResult(deviceReturnCode);
        }

        public Task INA219_Read(byte register, out int value)
        {
            byte[] temp = new byte[2];

            int i2cHandle = Open("/dev/i2c-1", OPEN_READ_WRITE);
            // mount the device at address 'device' for communication
            //int registerAddress = 0x62;
            int deviceReturnCode = Ioctl(i2cHandle, I2C_CLIENT, deviceaddress);

            deviceReturnCode = Read(i2cHandle, temp, 2);

            value = ((temp[0] * 256) + temp[1]);

            return Task.FromResult(deviceReturnCode);
        }



        public Task set_calibration_32V_2A()
        {
            current_lsb = .1F;  // Current LSB = 100uA per bit

            cal_value = 4096;

            power_lsb = .002;  // Power LSB = 2mW per bit

            //INA219_Write(REG_CALIBRATION, cal_value);
            dev.SetCalibration(cal_value,current_lsb);

            //bus_voltage_range = BUSV_RANGE_32V;
            //gain = GAIN_DIV_8_320MV;
            //bus_adc_resolution = ADCRES_12BIT_32S;
            //shunt_adc_resolution = ADCRES_12BIT_32S;
            //mode = MODE_SANDBVOLT_CONTINUOUS;

            //config = bus_voltage_range << 13 | gain << 11 | bus_adc_resolution << 7 | shunt_adc_resolution << 3 | mode;


            //INA219_Write(REG_CONFIG, config);

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


            //System.Device.I2c.I2cConnectionSettings settings = new System.Device.I2c.I2cConnectionSettings(1, 0x42);


            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                //dev =  new Ina219(settings);

                // await set_calibration_32V_2A();


                dev.SetCalibration(cal_value);


                //await INA219_Write(REG_CALIBRATION, cal_value);
                //await INA219_Read(REG_SHUNTVOLTAGE, out val);
                //if (val > 32767)
                //{
                //    val -= 65535;
                //}
                //shunt_voltage = (float)(val * 0.01);

                shunt_voltage = dev.ReadShuntVoltage();

                //await INA219_Write(REG_CALIBRATION, cal_value);
                //await INA219_Read(REG_BUSVOLTAGE, out val);
                // bus_voltage = (float)((val >> 3) * 0.004);

                bus_voltage = dev.ReadBusVoltage();

                //await INA219_Read(REG_CURRENT, out val);
                //if (val > 32767)
                //{
                //    val -= 65535;
                //}
                //current_ma = (float)(val * current_lsb);

                current_ma = dev.ReadCurrent().Milliamperes * current_lsb ;

                //await INA219_Write(REG_CALIBRATION, cal_value);
                //await INA219_Read(REG_POWER, out val);
                //if (val > 32767)
                //{
                //    val -= 65535;
                //}
                //power_w = (float)(val * power_lsb);

                dev.SetCalibration(cal_value, current_lsb);
                power_w = (double)(dev.ReadPower().Watts) * power_lsb;

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
