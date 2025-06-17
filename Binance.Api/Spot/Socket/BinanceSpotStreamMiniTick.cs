namespace Binance.Api.Spot;

/// <summary>
/// MiniTick info
/// </summary>
public abstract record BinanceSpotStreamMiniTickBase : BinanceSocketStreamEvent
{
    /// <summary>
    /// The symbol this data is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The current day close price. This is the latest price for this symbol.
    /// </summary>
    [JsonProperty("c")]
    public decimal LastPrice { get; set; }

    /// <summary>
    /// Todays open price
    /// </summary>
    [JsonProperty("o")]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// Todays high price
    /// </summary>
    [JsonProperty("h")]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// Todays low price
    /// </summary>
    [JsonProperty("l")]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// Total traded volume
    /// </summary>
    public abstract decimal Volume { get; set; }

    /// <summary>
    /// Total traded quote volume
    /// </summary>
    public abstract decimal QuoteVolume { get; set; }
}

/// <summary>
/// Stream mini tick
/// </summary>
public record BinanceSpotStreamMiniTick : BinanceSpotStreamMiniTickBase
{
    /// <inheritdoc/>
    [JsonProperty("v")]
    public override decimal Volume { get; set; }

    /// <inheritdoc/>
    [JsonProperty("q")]
    public override decimal QuoteVolume { get; set; }
}