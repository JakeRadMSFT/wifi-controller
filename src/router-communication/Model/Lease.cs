using System;
using System.Text.Json.Serialization;

public class Lease
{
    [JsonPropertyName(".id")]
    public string Id { get; set; }

    [JsonPropertyName("active-address")]
    public string ActiveAddress { get; set; }

    [JsonPropertyName("active-client-id")]
    public string ActiveClientId { get; set; }

    [JsonPropertyName("active-mac-address")]
    public string ActiveMacAddress { get; set; }

    [JsonPropertyName("active-server")]
    public string ActiveServer { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("address-lists")]
    public string AddressLists { get; set; }

    [JsonPropertyName("blocked")]
    public string Blocked { get; set; }

    [JsonPropertyName("client-id")]
    public string ClientId { get; set; }

    [JsonPropertyName("dhcp-option")]
    public string DhcpOption { get; set; }

    [JsonPropertyName("disabled")]
    public string Disabled { get; set; }

    [JsonPropertyName("dynamic")]
    public string Dynamic { get; set; }

    [JsonPropertyName("expires-after")]
    public string ExpiresAfter { get; set; }

    [JsonPropertyName("host-name")]
    public string HostName { get; set; }

    [JsonPropertyName("last-seen")]
    public string LastSeen { get; set; }

    [JsonPropertyName("mac-address")]
    public string MacAddress { get; set; }

    [JsonPropertyName("radius")]
    public string Radius { get; set; }

    [JsonPropertyName("server")]
    public string Server { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

}
