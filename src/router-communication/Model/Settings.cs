using System.Text.Json.Serialization;

public class Settings
{
    [JsonPropertyName("force-starlink-enabled")]
    public bool ForceStarlinkEnabled { get; set; }

    [JsonPropertyName("force-router-enabled")]
    public bool ForceRouterEnabled { get; set; }

    [JsonPropertyName("upload-speed-threshold")]
    public float UploadSpeedThreshold { get; set; }

    [JsonPropertyName("download-speed-threshold")]
    public float DownloadSpeedThreshold { get; set; }

    [JsonPropertyName("ignore-list")]
    public string[] IgnoreList { get; set; }
}