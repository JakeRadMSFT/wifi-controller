// See https://aka.ms/new-console-template for more information
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Device.Gpio;
using System.Text.Json;

// URL for HTTP GET request
string url = "https://raw.githubusercontent.com/JakeRadMSFT/wifi-controller/main/src/settings.json";

int StarlinkPin = 18;
int RouterPin = 16;
   //using GpioController controller = new GpioController();


    // Setup pins as outputs
    // controller.OpenPin(StarlinkPin, PinMode.Output);
    // controller.OpenPin(RouterPin, PinMode.Output);


    while (true)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    // Parse JSON using System.Text.Json
                    var data = JsonSerializer.Deserialize<JsonElement>(json);

                    // Get values from JSON
                    bool starlinkStatus = data.GetProperty("starlink").GetBoolean();
                    bool routerStatus = data.GetProperty("router").GetBoolean();

                    // Control GPIO pins based on values
                    //controller.Write(StarlinkPin, starlinkStatus ? PinValue.High : PinValue.Low);
                    //controller.Write(RouterPin, routerStatus ? PinValue.High : PinValue.Low);

                    // Get next check time
                    var nextCheck = DateTime.Parse(data.GetProperty("nextCheck").GetString());
                    var currentTime = DateTime.UtcNow;

                    // Calculate time until next check (in milliseconds)
                    var timeUntilNextCheck = (nextCheck - currentTime).TotalMilliseconds;

                    // Wait for 30 minutes
                    await Task.Delay(TimeSpan.FromMinutes(30));
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