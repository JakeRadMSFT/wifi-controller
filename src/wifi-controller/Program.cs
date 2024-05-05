using System.Device.Gpio;

// URL for HTTP GET request
const string remoteSettingsUrl = "https://raw.githubusercontent.com/JakeRadMSFT/wifi-controller/main/src/settings.json";

const int StarlinkPin = 24;
const int RouterPin = 14;
const int activitySampleIntervalMilliseconds = 500;

// Constants for router configuration
const string RouterIpAddress = "192.168.88.1";
const string RouterUsername = "rest";
const string RouterPassword = "Resty88";
const string RouterThumbprint = "F6BA38F384593FBA0197DD6DD9CEA466D92B77E1";

using GpioController controller = new GpioController();

// Setup pins as outputs
controller.OpenPin(StarlinkPin, PinMode.Output);
controller.OpenPin(RouterPin, PinMode.Output);

var remoteSettings = new RemoteSettings(remoteSettingsUrl);

var detectedActivity = false;

do
{
    // Turn on router and starlink
    controller.Write(StarlinkPin, PinValue.High);
    controller.Write(RouterPin, PinValue.High);

    // Wait some time for things to start up
    await Task.Delay(TimeSpan.FromSeconds(10));

    try
    {

        var settings = await remoteSettings.GetSettingsAsync();

        // Control GPIO pins based on values
        controller.Write(StarlinkPin, detectedActivity || settings.ForceStarlinkEnabled ? PinValue.High : PinValue.Low);
        controller.Write(RouterPin, detectedActivity || settings.ForceRouterEnabled ? PinValue.High : PinValue.Low);


        var router = new Router(RouterIpAddress, RouterUsername, RouterPassword, RouterThumbprint);
        detectedActivity = await router.HasNetworkActivity(activitySampleIntervalMilliseconds, settings.UploadSpeedThreshold, settings.DownloadSpeedThreshold, settings.IgnoreList.ToList());

        await Task.Delay(1000);

    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception: " + ex.Message);
    }

} while (true);