namespace Binance.Api.Futures;

/// <summary>
/// Default Notional and Leverage Brackets for a COIN-M pair
/// </summary>
public record BinanceFuturesPairBracket
{
    /// <summary>
    /// Pair
    /// </summary>
    [JsonProperty("pair")]
    public string Pair { get; set; } = string.Empty;

    /// <summary>
    /// Brackets
    /// </summary>
    [JsonProperty("brackets")]
    public List<BinanceFuturesBracket> Brackets { get; set; } = [];
}
