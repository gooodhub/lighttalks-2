using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.Devices.Gpio;
using Windows.System.Threading;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace BackgroundApplication
{
    public sealed class StartupTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;
        private GpioPin _greenPin;
        private GpioPin _yellowPin;
        private GpioPin _redPin;
        private ThreadPoolTimer _timer;

        public int Counter { get; set; } = 0;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // Permet à la Task de se poursuivre après la fin de Run 
            _deferral = taskInstance.GetDeferral();

            InitGpio();

            _timer = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick, TimeSpan.FromMilliseconds(100));
        }

        private void InitGpio()
        {
            _greenPin = GpioController.GetDefault().OpenPin(5);
            _greenPin.Write(GpioPinValue.Low);
            // Considère la Pin comme une sortie
            _greenPin.SetDriveMode(GpioPinDriveMode.Output);

            _yellowPin = GpioController.GetDefault().OpenPin(6);
            _yellowPin.Write(GpioPinValue.Low);
            _yellowPin.SetDriveMode(GpioPinDriveMode.Output);

            _redPin = GpioController.GetDefault().OpenPin(13);
            _redPin.Write(GpioPinValue.Low);
            _redPin.SetDriveMode(GpioPinDriveMode.Output);
        }

        private void Timer_Tick(ThreadPoolTimer timer)
        {
            switch (Counter)
            {
                case 0: _greenPin.Write(GpioPinValue.High);
                    Counter++;
                    break;
                case 1: _yellowPin.Write(GpioPinValue.High);
                    Counter++;
                    break;
                case 2: _redPin.Write(GpioPinValue.High);
                    Counter++;
                    break;
                case 3: _greenPin.Write(GpioPinValue.Low);
                    _yellowPin.Write(GpioPinValue.Low);
                    _redPin.Write(GpioPinValue.Low);
                    Counter = 0;
                    break;
            }
        }
    }
}
