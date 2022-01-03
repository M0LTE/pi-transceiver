using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace rig_controller.Services
{
    public class FlexKnob
    {
        /// <summary>
        /// Blocks
        /// </summary>
        public void Run()
        {
            if (OnButtonPush == null || OnRotate == null)
            {
                throw new InvalidOperationException($"Subscribe to both {nameof(OnButtonPush)} and {nameof(OnRotate)}");
            }

            string[] portNames = SerialPort.GetPortNames();

            foreach (string port in portNames)
            {
                List<byte> buffer = new List<byte>();

                using var sp = new SerialPort(port, 9600);

                try
                {
                    sp.Open();
                }
                catch (Exception)
                {
                    continue;
                }

                sp.ReadTimeout = 500;

                var failures = 0;
                while (true)
                {
                    int i;
                    try
                    {
                        i = sp.ReadByte();
                        buffer.Add((byte)i);
                        if (IsFlexKnobStartup(buffer))
                        {
                            RunKnob(sp);
                            return;
                        }
                    }
                    catch (TimeoutException)
                    {
                        failures++;
                        if (failures == 3)
                        {
                            break;
                        }
                    }
                }
            }

            throw new Exception("Flex knob not found, is it connected? Has something else opened its serial port already?");
        }

        void RunKnob(SerialPort sp)
        {
            List<byte> buffer = new List<byte>();

            while (true)
            {
                byte b;

                try
                {
                    b = (byte)sp.ReadByte();
                    buffer.Add(b);
                    if (b == ';')
                    {
                        InterpretBuffer(buffer);
                    }
                }
                catch (TimeoutException) { }
            }
        }

        const string flexStartupString = "F0304;";
        static byte[] flexBytes = Encoding.ASCII.GetBytes(flexStartupString);

        static bool IsFlexKnobStartup(List<byte> buffer)
        {
            if (buffer.Count() != flexBytes.Length)
                return false;

            for (int i = 0; i < buffer.Count(); i++)
            {
                if (buffer[i] != flexBytes[i])
                    return false;
            }

            return true;
        }

        void InterpretBuffer(List<byte> buffer)
        {
            if (buffer.Last() != (byte)';')
                return;

            if (buffer[0] == 'U' || buffer[0] == 'D')
            {
                ProcessUpDown(buffer);
            }
            else if (buffer[0] == 'X' || buffer[0] == 'S' || buffer[0] == 'L' || buffer[0] == 'C')
            {
                ProcessButton(buffer);
            }
            else if (IsFlexKnobStartup(buffer))
            {
                // do nothing, it seems to be a bit variable how many times this gets sent
            }
            else
            {
                Console.WriteLine($"Unknown command {Encoding.ASCII.GetString(buffer.ToArray())}");
            }

            buffer.Clear();
        }

        void ProcessButton(List<byte> buffer)
        {
            InterpretButton(buffer, out Button btn, out TapType tap);

            OnButtonPush(btn, tap);
        }

        void InterpretButton(List<byte> buffer, out Button button, out TapType tapType)
        {
            char pressLen = (char)buffer.Take(buffer.Count() - 1).Last();

            if (buffer[0] == 'X')
            {
                if (buffer[1] == '1')
                {
                    button = Button.AUX1;
                }
                else if (buffer[1] == '2')
                {
                    button = Button.AUX2;
                }
                else if (buffer[1] == '3')
                {
                    button = Button.AUX3;
                }
                else
                {
                    throw new NotImplementedException($"Button X{(char)buffer[1]} not implemented");
                }
            }
            else if (buffer[0] == 'S' || buffer[0] == 'L' || buffer[0] == 'C')
            {
                button = Button.Knob;
            }
            else
            {
                throw new NotImplementedException($"Button {(char)buffer[0]} not implemented");
            }

            if (pressLen == 'S')
            {
                tapType = TapType.Single;
            }
            else if (pressLen == 'C')
            {
                tapType = TapType.Double;
            }
            else if (pressLen == 'L')
            {
                tapType = TapType.Long;
            }
            else
            {
                throw new NotImplementedException($"Tap type {(char)pressLen} not implemented");
            }
        }

        void ProcessUpDown(List<byte> buffer)
        {
            InterpretUpDown(buffer, out Direction dir, out int speed);

            OnRotate(dir, speed);
        }

        void InterpretUpDown(List<byte> buffer, out Direction dir, out int speed)
        {
            if (buffer[0] == 'U')
            {
                dir = Direction.Up;
            }
            else if (buffer[0] == 'D')
            {
                dir = Direction.Down;
            }
            else
            {
                throw new NotImplementedException($"Direction {(char)buffer[0]} not implemented");
            }

            if (buffer.Count() == 2)
            {
                speed = 1;
            }
            else
            {
                if (!int.TryParse(new string(new[] { (char)buffer[1], (char)buffer[2] }), out speed))
                {
                    throw new NotImplementedException($"Invalid speed {(char)buffer[1]}{(char)buffer[2]}");
                }

                OnRotate(dir, speed);
            }
        }

        /// <summary>
        /// Any of the buttons (and the knob) can be pushed any way.
        /// </summary>
        public Action<Button, TapType> OnButtonPush { get; set; }

        /// <summary>
        /// Speed seems to be 1-8.
        /// </summary>
        public Action<Direction, int> OnRotate { get; internal set; }
    }

    public enum Button
    {
        AUX1, AUX2, AUX3, Knob
    }

    public enum TapType
    {
        Single, Double, Long
    }

    public enum Direction
    {
        Up, Down
    }
}
