namespace Binance.Api.Wallet;

/// <summary>
/// Account status info
/// </summary>
public record BinanceWalletAccountStatus
{
    /// <summary>
    /// The result status
    /// </summary>
    [JsonProperty("data")]
    public string Data { get; set; } = "";
}
