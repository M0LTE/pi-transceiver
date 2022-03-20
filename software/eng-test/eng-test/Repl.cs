using rig_controller.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eng_test
{
    internal class Repl
    {
        private readonly IGpioService gpioService;

        public Repl(IGpioService gpioService)
        {
            this.gpioService = gpioService;
        }

        public async Task Go()
        {
            Console.Clear();
            Console.WriteLine($"RADARC pi-transceiver engineering test program v{BuildInfo.GetVersion()}");

            while (true)
            {
                PrintMenu();
                Console.Write("> ");

                var input = Console.ReadLine() ?? "";

                switch (Interpret(input))
                {
                    case Command.SetGpio:
                        await HandleSetGpio();
                        continue;

                    case Command.Quit:
                        return;

                    case Command.Unknown:
                        continue;
                }

                Console.WriteLine();
            }
        }

        private async Task HandleSetGpio()
        {
            try
            {
                Console.Write("Which pin? ");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out var pin) || pin < 1 || pin > 40)
                {
                    Console.WriteLine("Invalid pin");
                    return;
                }

                Console.WriteLine("1 or 0? ");
                input = Console.ReadLine();

                bool state;
                if (input == "0" || input == "1")
                {
                    state = input == "1";
                }
                else
                {
                    Console.WriteLine("Invalid state, need 0 or 1");
                    return;
                }

                try
                {
                    await gpioService.SetGpio(pin, state);
                }
                catch (Exception ex)
                {
                    PrintException(ex);
                    return;
                }

                using (SetColour(ConsoleColor.Green))
                {
                    Console.WriteLine($"Set pin {pin} to {(state ? "1" : "0")}");
                }
            }
            finally
            {
                await Task.Delay(1000);
            }
        }

        private void PrintException(Exception ex)
        {
            using (SetColour(ConsoleColor.Red))
            {
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            }
        }

        private IDisposable SetColour(ConsoleColor colour)
        {
            var oldColour = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            return new ColourResetter(oldColour);
        }

        private class ColourResetter : IDisposable
        {
            private ConsoleColor oldColour;

            public ColourResetter(ConsoleColor oldColour)
            {
                this.oldColour = oldColour;
            }

            public void Dispose()
            {
                Console.ForegroundColor = oldColour;
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("    g   Set GPIO");
            Console.WriteLine("    q   Quit");
            Console.WriteLine();
        }

        private Command Interpret(string input)
        {
            var normalised = input.Trim().ToLower();

            return normalised switch
            {
                "g" => Command.SetGpio,
                "q" => Command.Quit,
                _ => Command.Unknown
            };
        }

        private enum Command
        {
            Unknown,
            Quit,
            SetGpio,
        }
    }
}
