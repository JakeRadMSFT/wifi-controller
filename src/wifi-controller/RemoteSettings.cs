using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

public class RemoteSettings
{
    private readonly string url;

    public RemoteSettings(string url)
    {
        this.url = url;
    }

    public async Task<Settings> GetSettingsAsync()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Create a request message
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Add headers to prevent caching
                request.Headers.CacheControl = new CacheControlHeaderValue
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

                    // Deserialize JSON to custom class
                    var settings = JsonSerializer.Deserialize<Settings>(json);
                    return settings;
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

        return null;
    }
}
