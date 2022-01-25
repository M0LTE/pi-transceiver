
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

        [DllImport("libc.so.6", EntryPoint = "open")]
        private static extern int Open(string fileName, int mode);

        [DllImport("libc.so.6", EntryPoint = "ioctl", SetLastError = true)]
        private static extern int Ioctl(int fd, int request, int data);

        [DllImport("libc.so.6", EntryPoint = "read", SetLastError = true)]
        private static extern int Read(int handle, byte[] data, int length);

        [DllImport("libc.so.6", EntryPoint = "write", SetLastError = true)]
        private static extern int Write(int handle, byte[] data, int length);


        // externals for the i2c libraries


        private readonly ILogger<i2cDacService> _logger;

        public i2cDacService(ILogger<i2cDacService> logger)
        {
            _logger = logger;

        }

        public Task SetDAC(int device, out int value, bool writeEEPROM, UInt16 voltage)
        {

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                _logger.LogTrace($"No linux i2c, skipping SetDAC, we are running in Windows.");
                value = -1;
                return Task.CompletedTask;

            }

            int i2cHandle = Open("/dev/i2c-1", OPEN_READ_WRITE);
            // mount the device at address 'device' for communication
            //int registerAddress = 0x62;
            int deviceReturnCode = Ioctl(i2cHandle, I2C_CLIENT, device);

            value = deviceReturnCode;

            byte[] data = new byte[3];

            data[0] = writeEEPROM ? MCP4726_CMD_WRITEDACEEPROM : MCP4726_CMD_WRITEDAC;

            data[1] = (byte)(voltage >> 4); // Another way
            data[2] = (byte)((voltage & 15) << 4);

            deviceReturnCode = Write(i2cHandle, data, 3);

            return Task.CompletedTask;
        }
    }


}
