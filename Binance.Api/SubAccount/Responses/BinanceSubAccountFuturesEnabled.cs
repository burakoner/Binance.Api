namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account futures trading enabled
/// </summary>
public record BinanceSubAccountFuturesEnabled
{
    /// <summary>
    /// Email of the account
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Whether futures trading is enabled
    /// </summary>
    [JsonProperty("isFuturesEnabled")]
    public bool IsFuturesEnabled { get; set; }
}
