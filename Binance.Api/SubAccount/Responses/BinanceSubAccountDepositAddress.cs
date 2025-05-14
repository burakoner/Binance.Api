namespace Binance.Api.SubAccount;

/// <summary>
/// Deposit address info for a sub-account
/// </summary>
public record BinanceSubAccountDepositAddress
{
    /// <summary>
    /// The deposit address
    /// </summary>
    [JsonProperty("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Asset type
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Tag for the deposit address
    /// </summary>
    [JsonProperty("tag")]
    public string Tag { get; set; } = string.Empty;

    /// <summary>
    /// Url
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}
