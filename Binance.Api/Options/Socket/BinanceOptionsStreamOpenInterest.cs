namespace Binance.Api.Options;

/// <summary>
/// Binance Options Web Socket Stream Open Interest
/// </summary>
public record BinanceOptionsStreamOpenInterest : BinanceSocketStreamEvent
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Open interest in contracts
    /// </summary>
    [JsonProperty("o")]
    public decimal OpenInterestInContracts { get; set; }

    /// <summary>
    /// Open interest in USDT
    /// </summary>
    [JsonProperty("h")]
    public decimal OpenInterestInUSDT { get; set; }
}
