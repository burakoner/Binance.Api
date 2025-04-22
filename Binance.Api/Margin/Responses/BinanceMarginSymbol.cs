namespace Binance.Api.Margin;

/// <summary>
/// Margin pair info
/// </summary>
public record BinanceMarginSymbol
{
    /// <summary>
    /// Base asset of the pair
    /// </summary>
    [JsonProperty("base")]
    public string BaseAsset { get; set; } = "";

    /// <summary>
    /// Quote asset of the pair
    /// </summary>
    [JsonProperty("quote")]
    public string QuoteAsset { get; set; } = "";

    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Is buying allowed
    /// </summary>
    public bool IsBuyAllowed { get; set; }

    /// <summary>
    /// Is selling allowed
    /// </summary>
    public bool IsSellAllowed { get; set; }

    /// <summary>
    /// Is margin trading
    /// </summary>
    public bool IsMarginTrade { get; set; }

    /// <summary>
    /// Symbol name
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Time at which the symbol gets delisted
    /// </summary>
    [JsonProperty("delistTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? DelistTime { get; set; }
}
