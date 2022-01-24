//using Unosquare.RaspberryIO;
using System;
using System.Runtime.InteropServices;


namespace rig_controller.Services
{
    
    public class i2cDacService 
    {
        const byte MCP4726_CMD_WRITEDAC = 0x40; // Writes data to the DAC
        const byte MCP4726_CMD_WRITEDACEEPROM = 0x60; // Writes data to the DAC and the EEPROM (persisting the assigned value after reset)

        // constants for i2c
        private static int OPEN_READ_WRITE = 2;
        private static int I2C_CLIENT = 0x0703;

        // externals for the i2c libraries
        [DllImport("libc.so.6", EntryPoint = "open")]
        private static extern int Open(string fileName, int mode);
        [DllImport("libc.so.6", EntryPoint = "ioctl", SetLastError = true)]
        private static extern int Ioctl(int fd, int request, int data);
        [DllImport("libc.so.6", EntryPoint = "read", SetLastError = true)]
        private static extern int Read(int handle, byte[] data, int length);
        [DllImport("libc.so.6", EntryPoint = "write", SetLastError = true)]
        private static extern int Write(int handle, byte[] data, int length);

        private readonly ILogger<i2cDacService> _logger;

        public i2cDacService(ILogger<i2cDacService> logger)
        {
            _logger = logger;

            //Pi.Init<Unosquare.WiringPi.BootstrapWiringPi>();
        }

        public Task SetDAC(int device, out int value, bool writeEEPROM,UInt16 voltage)
        {
            //double scale = await GetScale(device, channel);

            //var myDevice =  Pi.I2C.AddDevice(0x62);

            int i2cHandle = Open("/dev/i2c-1", OPEN_READ_WRITE);
            // mount the device at address 0x1A for communication
            int registerAddress = 0x62;
            int deviceReturnCode = Ioctl(i2cHandle, I2C_CLIENT, registerAddress);

            value = deviceReturnCode;

            byte[] data = new byte[3];

            data[0] = writeEEPROM ? MCP4726_CMD_WRITEDACEEPROM : MCP4726_CMD_WRITEDAC;

            data[1]= (byte)(voltage >> 4); // Another way
            data[2] = (byte)((voltage & 15) << 4);

           

           

            deviceReturnCode = Write(i2cHandle, data, 3);





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
