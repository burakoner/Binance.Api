namespace Binance.Api.Wallet;

/// <summary>
/// The status of Binance
/// </summary>
public record BinanceWalletSystemStatus
{
    /// <summary>
    /// Status
    /// </summary>
    public BinanceSystemStatus Status { get; set; }

    /// <summary>
    /// Additional info
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = "";
}
