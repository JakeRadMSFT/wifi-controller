using System.Text.Json;

internal class Router
{
    private string apiUrl;
    private HttpClient client;

    public Router(string ipAddress, string username, string password, string thumbprint)
    {
        apiUrl = $"https://{ipAddress}/rest";

        this.client = CreateAuthenticatedClient(username, password, thumbprint);
    }

    public async Task<Lease[]> GetLeasesAsync()
    {
        var leaseUrl = $"{apiUrl}/ip/dhcp-server/lease";

        HttpResponseMessage response = await client.GetAsync(leaseUrl);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();

            // Parse the JSON response
            return JsonSerializer.Deserialize<Lease[]>(responseBody);
        }
        else
        {
            throw new Exception("Failed to call the REST API. Status code: " + response.StatusCode);
        }
    }

    public async Task<InterfaceData[]> GetInterfaceAsync()
    {
        var leaseUrl = $"{apiUrl}/interface";

        HttpResponseMessage response = await client.GetAsync(leaseUrl);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();

            // Parse the JSON response
            return JsonSerializer.Deserialize<InterfaceData[]>(responseBody);
        }
        else
        {
            throw new Exception("Failed to call the REST API. Status code: " + response.StatusCode);
        }
    }

    public async Task<bool> HasNetworkActivity(int activitySampleIntervalMilliseconds, float uploadThreshold, float downloadThreshold, List<string> macAddressIgnoreList)
    {

        var leases = await GetLeasesAsync();

        // Check for leases with MAC addresses not in the list
        var connectedClients = leases.Where(lease => !macAddressIgnoreList.Contains(lease.MacAddress));

        if (connectedClients.Count() > 0)
        {
            return true;
        }

        var bridge = (await GetInterfaceAsync()).Where(x => x.Name == "bridge").FirstOrDefault();

        float initialReceiveByte = long.Parse(bridge.RxByte);
        float initialTransmitByte = long.Parse(bridge.TxByte);

        await Task.Delay(activitySampleIntervalMilliseconds);

        bridge = (await GetInterfaceAsync()).Where(x => x.Name == "bridge").FirstOrDefault();

        float finalReceiveByte = long.Parse(bridge.RxByte);
        float finalTransmitByte = long.Parse(bridge.TxByte);

        var uploading = (finalReceiveByte - initialReceiveByte) * 8 / (activitySampleIntervalMilliseconds / 1000) > uploadThreshold;
        var downloading = (finalTransmitByte - initialTransmitByte) * 8 / (activitySampleIntervalMilliseconds / 1000) > downloadThreshold;

        return uploading || downloading;
    }

    private HttpClient CreateAuthenticatedClient(string username, string password, string thumbprint)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                // Check if the certificate matches the expected thumbprint

                if (cert.GetCertHashString().Equals(thumbprint, StringComparison.OrdinalIgnoreCase))
                {
                    return true; // Accept the certificate
                }
                else
                {
                    return false; // Reject the certificate
                }
            }
        };
        HttpClient client = new HttpClient(handler);

        string authHeaderValue = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeaderValue);
        return client;
    }
}
