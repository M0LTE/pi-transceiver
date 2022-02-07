
using System.Runtime.InteropServices;


namespace rig_controller.Services
{

    public class i2cDacService
    {
        const byte MCP4726_CMD_WRITEDAC = 0x40; // Writes data to the DAC
        const byte MCP4726_CMD_WRITEDACEEPROM = 0x60; // Writes data to the DAC and the EEPROM (persisting the assigned value after reset)
        public const byte MCP4726_DEFAULT_ADDRESS = 0x62; // Default i2c address for this DAC

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

        /// <summary>
        ///    Sets the output voltage to a fraction of source vref (5.1v).  (output level
        ///       can be 0..4095)
        /// 
        ///     deviceAddress  - i2c address of the DAC to update. Default address 0x62 is in constant MCP4726_DEFAULT_ADDRESS
        /// 
        ///
        ///     writeEEPROM
        ///            If this value is true, 'output' will also be written
        ///            to the MCP4725's internal non-volatile memory, meaning
        ///            that the DAC will retain the current voltage output
        ///            after power-down or reset.
        ///            
        ///     outputLevel
        ///            The 12-bit value representing the relationship between
        ///            the DAC's input voltage and its output voltage.
        ///            
        ///     returnValue
        ///            Whether the i2c command was successful, 0 is success
        ///
        /// </summary>

        public Task SetDAC(int deviceAddress, bool writeEEPROM, UInt16 outputLevel, out int returnValue)
        {

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                _logger.LogTrace($"No linux i2c, skipping SetDAC, we are running in Windows.");
                returnValue = -1;
                return Task.CompletedTask;

            }

            if (outputLevel > 4095)
            {
                outputLevel = 4095;
            }

            int i2cHandle = Open("/dev/i2c-1", OPEN_READ_WRITE);
            // mount the device at address 'device' for communication
            //int registerAddress = 0x62;
            int deviceReturnCode = Ioctl(i2cHandle, I2C_CLIENT, deviceAddress);

            if (deviceReturnCode != -1)
            {

                byte[] data = new byte[3];

                data[0] = writeEEPROM ? MCP4726_CMD_WRITEDACEEPROM : MCP4726_CMD_WRITEDAC;

                data[1] = (byte)(outputLevel >> 4); // Another way
                data[2] = (byte)((outputLevel & 15) << 4);

                deviceReturnCode = Write(i2cHandle, data, 3);

            }

            returnValue = deviceReturnCode;


            return Task.CompletedTask;
        }
    }


}
