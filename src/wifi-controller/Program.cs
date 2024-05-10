using starlink_controller;
using System.Device.Gpio;

const int StarlinkPin = 24;
const int RouterPin = 14;

using GpioController controller = new GpioController();

// Setup pins as outputs
controller.OpenPin(StarlinkPin, PinMode.Output);
controller.OpenPin(RouterPin, PinMode.Output);

// Turn on router and starlink
controller.Write(StarlinkPin, PinValue.High);
controller.Write(RouterPin, PinValue.High);

// Wait some time for things to start up
await Task.Delay(TimeSpan.FromSeconds(10));

try
{
    var startlinkController = new Controller();
    await startlinkController.UpdateAsync();

    // Control GPIO pins based on values
    controller.Write(StarlinkPin, startlinkController.StarlinkEnabled ? PinValue.High : PinValue.Low);
    controller.Write(RouterPin, startlinkController.RouterEnabled ? PinValue.High : PinValue.Low);
}
catch (Exception ex)
{
    Console.WriteLine("Exception: " + ex.Message);
}