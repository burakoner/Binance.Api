namespace Binance.Api.Wallet.Responses;

/// <summary>
/// Account status info
/// </summary>
public record BinanceAccountStatus
{
    /// <summary>
    /// The result status
    /// </summary>
    [JsonProperty("data")]
    public string Data { get; set; } = "";
}
