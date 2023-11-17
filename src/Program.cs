using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Device.Gpio;
using System.Text.Json;

// URL for HTTP GET request
string url = "https://raw.githubusercontent.com/JakeRadMSFT/wifi-controller/main/src/settings.json";

int StarlinkPin = 18;
int RouterPin = 16;

using GpioController controller = new GpioController();


// Setup pins as outputs
controller.OpenPin(StarlinkPin, PinMode.Output);
controller.OpenPin(RouterPin, PinMode.Output);


while (true)
{
    // Turn on router and starlink
    controller.Write(StarlinkPin, PinValue.High);
    controller.Write(RouterPin, PinValue.High);

    // Wait some time for things to start up
    await Task.Delay(TimeSpan.FromMinutes(5));

    try
    {
        using (HttpClient client = new HttpClient())
        {
            // Create a request message
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // Add headers to prevent caching
            request.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true,
                MustRevalidate = true,
                MaxAge = TimeSpan.Zero
            };

            // Send the request
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                // Parse JSON using System.Text.Json
                var data = JsonSerializer.Deserialize<JsonElement>(json);

                // Get values from JSON
                bool starlinkStatus = data.GetProperty("starlink").GetBoolean();
                bool routerStatus = data.GetProperty("router").GetBoolean();

                // Control GPIO pins based on values
                controller.Write(StarlinkPin, starlinkStatus ? PinValue.High : PinValue.Low);
                controller.Write(RouterPin, routerStatus ? PinValue.High : PinValue.Low);

                // Get next check time
                var nextCheck = DateTime.Parse(data.GetProperty("nextCheck").GetString());
                var currentTime = DateTime.UtcNow;

                // Calculate time until next check (in milliseconds)
                var timeUntilNextCheck = (nextCheck - currentTime).TotalMilliseconds;

                // Wait for 30 minutes
                await Task.Delay(TimeSpan.FromMinutes(15));
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception: " + ex.Message);
    }
}