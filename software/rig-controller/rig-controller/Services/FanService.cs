using Microsoft.Extensions.Logging;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Device.Pwm.Drivers;
using System.Threading.Tasks;


namespace rig_controller.Services
{

    public interface IFanService : IDisposable
    {
        Task PWMFanSimpleExample();
    }
    public class FanService : IFanService
    {
        const int gpioPWMPin = 12;
        const int frequency = 25;
        const double initialDutyCycle = 0;

        private readonly ILogger<FanService> _logger;

        public FanService(ILogger<FanService> logger)
        {
            _logger = logger;
            //InitializeSystem();

        }

        public Task PWMFanSimpleExample()
        {
            _logger.LogInformation("Starting PWM Controller - Simple Demo");
            using (var controller = new SoftwarePwmChannel(gpioPWMPin,frequency,initialDutyCycle))
            {
                double dutyCycle = 1;
                controller.Start();

                controller.DutyCycle = dutyCycle;

                _logger.LogInformation("Duty cycle " + dutyCycle);
                Task.Delay(new TimeSpan(0, 0, 10)).Wait(); //10 second wait to give fan time to power up
                ReadTachometer();

                dutyCycle = 0.7;
                controller.DutyCycle = dutyCycle;
                _logger.LogInformation("Duty cycle " + dutyCycle);
                Task.Delay(new TimeSpan(0, 0, 2)).Wait(); //2 second wait
                ReadTachometer();

                dutyCycle = 0.3;
                controller.DutyCycle = dutyCycle;
                _logger.LogInformation("Duty cycle " + dutyCycle);
                Task.Delay(new TimeSpan(0, 0, 2)).Wait(); //2 second wait
                ReadTachometer();

                controller.DutyCycle = 0;
                controller.Stop();
                controller.Dispose();
                _logger.LogInformation("Finished - Simple Demo");

                
            }

            return Task.CompletedTask;

           
        }

        public void Dispose()
        {
           
        }

        private void ReadTachometer()
        {
            var pin = 5;
            var pulses = 0;
            var startTime = DateTime.Now;
            var sampleMilliseconds = 5000;
            PinChangeEventHandler onPinEvent = (object sender, PinValueChangedEventArgs args) => { pulses++; };
            using (var controller = new GpioController())
            {
                controller.OpenPin(pin, PinMode.InputPullUp);
                controller.RegisterCallbackForPinValueChangedEvent(pin, PinEventTypes.Rising, onPinEvent);
                Task.Delay(new TimeSpan(0, 0, 0, 0, sampleMilliseconds)).Wait(); //wait
                var milliSeconds = (DateTime.Now - startTime).TotalMilliseconds;
                var revsPerSecond = (pulses / 2) / (milliSeconds / 1000);
                var rpm = Convert.ToInt32(revsPerSecond * 60);
                controller.UnregisterCallbackForPinValueChangedEvent(pin, onPinEvent);
                _logger.LogInformation($"Fan is running at {rpm} revolutions per minute");
            }
        }
    }
}
