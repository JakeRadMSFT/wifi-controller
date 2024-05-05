using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class InterfaceData
{
    [JsonPropertyName(".id")]
    public string Id { get; set; }

    [JsonPropertyName("actual-mtu")]
    public string ActualMtu { get; set; }

    [JsonPropertyName("default-name")]
    public string DefaultName { get; set; }

    [JsonPropertyName("disabled")]
    public string Disabled { get; set; }

    [JsonPropertyName("fp-rx-byte")]
    public string FpRxByte { get; set; }

    [JsonPropertyName("fp-rx-packet")]
    public string FpRxPacket { get; set; }

    [JsonPropertyName("fp-tx-byte")]
    public string FpTxByte { get; set; }

    [JsonPropertyName("fp-tx-packet")]
    public string FpTxPacket { get; set; }

    [JsonPropertyName("l2mtu")]
    public string L2Mtu { get; set; }

    [JsonPropertyName("last-link-down-time")]
    public string LastLinkDownTime { get; set; }

    [JsonPropertyName("last-link-up-time")]
    public string LastLinkUpTime { get; set; }

    [JsonPropertyName("link-downs")]
    public string LinkDowns { get; set; }

    [JsonPropertyName("mac-address")]
    public string MacAddress { get; set; }

    [JsonPropertyName("max-l2mtu")]
    public string MaxL2Mtu { get; set; }

    [JsonPropertyName("mtu")]
    public string Mtu { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("running")]
    public string Running { get; set; }

    [JsonPropertyName("rx-byte")]
    public string RxByte { get; set; }

    [JsonPropertyName("rx-drop")]
    public string RxDrop { get; set; }

    [JsonPropertyName("rx-error")]
    public string RxError { get; set; }

    [JsonPropertyName("rx-packet")]
    public string RxPacket { get; set; }

    [JsonPropertyName("tx-byte")]
    public string TxByte { get; set; }

    [JsonPropertyName("tx-drop")]
    public string TxDrop { get; set; }

    [JsonPropertyName("tx-error")]
    public string TxError { get; set; }

    [JsonPropertyName("tx-packet")]
    public string TxPacket { get; set; }

    [JsonPropertyName("tx-queue-drop")]
    public string TxQueueDrop { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
