using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Device.I2c;

var cmd = new RootCommand
{
    new Argument<int>("bus", "The I2C bus id"),
    new Argument<int>("device", "The I2C device id"),
};

cmd.Description = "A simple app to directly manipulate I2C devices via System.Device.I2C";

cmd.Handler = CommandHandler.Create<int, int>(HandleGreeting);

return cmd.Invoke(args);

static void HandleGreeting(int bus, int device)
{
    Console.WriteLine($"Connecting to i2c bus {bus}, device {device}...");

    var settings = new I2cConnectionSettings(bus, device);

    I2cDevice dev;

    try
    {
        dev = I2cDevice.Create(settings);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.GetBaseException().Message);
        return;
    }

    Console.WriteLine("... connected");

    Console.WriteLine("Commands: read, write [hex byte]");
    while (true)
    {
        var cmd = Console.ReadLine();
        if (cmd == null) continue;

        if (cmd == "r" || cmd == "read")
        {

            byte b;

            try
            {
                b = dev.ReadByte();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                continue;
            }

            Console.WriteLine($"0x{BitConverter.ToString(new[] { b }).ToString().ToLower()}");
        }
        else if (cmd.StartsWith("write ") || cmd.StartsWith("w "))
        {
            var parts = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1);
            
            foreach (var p in parts)
            {
                int i;

                try
                {
                    i = Convert.ToInt32(p, 16);
                }
                catch
                {
                    Console.WriteLine("invalid byte, specify 00-ff");
                    break;
                }

                try
                {
                    dev.WriteByte((byte)i);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                    break;
                }
            }
        }
        else if (cmd == "q" || cmd == "quit")
        {
            return;
        }
    }
}