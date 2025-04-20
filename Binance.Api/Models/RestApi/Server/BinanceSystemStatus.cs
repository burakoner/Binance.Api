namespace Binance.Api.Models.RestApi.Server;

/// <summary>
/// The status of Binance
/// </summary>
public record BinanceSystemStatus
{
    /// <summary>
    /// Status
    /// </summary>
    [JsonConverter(typeof(SystemStatusConverter))]
    public SystemStatus Status { get; set; }
    /// <summary>
    /// Additional info
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = "";
}
