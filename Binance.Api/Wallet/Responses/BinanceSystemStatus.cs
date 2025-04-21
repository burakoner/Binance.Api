namespace Binance.Api.Wallet;

/// <summary>
/// The status of Binance
/// </summary>
public record BinanceSystemStatus
{
    /// <summary>
    /// Status
    /// </summary>
    public SystemStatus Status { get; set; }

    /// <summary>
    /// Additional info
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = "";
}
