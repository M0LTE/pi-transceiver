using eng_test;
using rig_controller.Services;
using System.Runtime.InteropServices;

IGpioService gpio;
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    gpio = new NullGpioService();
}
else
{
    gpio = new NativeGpioService();
}

var repl = new Repl(gpio);
await repl.Go();