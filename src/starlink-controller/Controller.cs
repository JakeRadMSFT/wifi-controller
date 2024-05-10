using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace starlink_controller
{
    public class Controller
    {
        const string remoteSettingsUrl = "https://raw.githubusercontent.com/JakeRadMSFT/wifi-controller/main/src/wifi-controller/settings.json";
        const int activitySampleIntervalMilliseconds = 500;

        // Constants for router configuration
        const string RouterIpAddress = "192.168.88.1";
        const string RouterUsername = "rest";
        const string RouterPassword = "Resty88";
        const string RouterThumbprint = "F6BA38F384593FBA0197DD6DD9CEA466D92B77E1";

        public bool StarlinkEnabled { get; set; }
        public bool RouterEnabled { get; set; }

        public async Task UpdateAsync()
        {
            var remoteSettings = new RemoteSettings(remoteSettingsUrl);
            try
            {
                var settings = await remoteSettings.GetSettingsAsync();

                var router = new Router(RouterIpAddress, RouterUsername, RouterPassword, RouterThumbprint);
                var detectedActivity = await router.HasNetworkActivity(activitySampleIntervalMilliseconds, settings.UploadSpeedThreshold, settings.DownloadSpeedThreshold, settings.IgnoreList.ToList());

                StarlinkEnabled = (!settings.ForceStarlinkDisabled && detectedActivity) || settings.ForceStarlinkEnabled;
                RouterEnabled = (!settings.ForceRouterDisabled && detectedActivity) || settings.ForceRouterEnabled;

                await Task.Delay(1000);

                if (StarlinkEnabled)
                {
                    if (settings.ForceStarlinkEnabled)
                    {
                        Console.WriteLine("Starlink is enabled due to a force enable setting.");
                    }
                    else
                    {
                        Console.WriteLine("Starlink is enabled due to detected network activity.");
                    }
                }
                else
                {
                    if (settings.ForceStarlinkDisabled)
                    {
                        Console.WriteLine("Starlink is disabled due to a force disable setting.");
                    }
                    else
                    {
                        Console.WriteLine("Starlink is disabled.");
                    }
                }

                if (RouterEnabled)
                {
                    if (settings.ForceRouterEnabled)
                    {
                        Console.WriteLine("Router is enabled due to a force enable setting.");
                    }
                    else
                    {
                        Console.WriteLine("Router is enabled due to detected network activity.");
                    }
                }
                else
                {
                    if (settings.ForceRouterDisabled)
                    {
                        Console.WriteLine("Router is disabled due to a force disable setting.");
                    }
                    else
                    {
                        Console.WriteLine("Router is disabled.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

    }
}
