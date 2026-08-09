namespace Binance.Api.Futures;

/// <summary>
/// Futures 24-hour mini ticker update
/// </summary>
public record BinanceFuturesStreamMiniTick : BinanceFuturesStreamEvent
{
    /// <summary>
    /// The symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The pair
    /// </summary>
    [JsonProperty("ps")]
    public string Pair { get; set; } = string.Empty;

    /// <summary>
    /// Last price
    /// </summary>
    [JsonProperty("c")]
    public decimal LastPrice { get; set; }

    /// <summary>
    /// Open price
    /// </summary>
    [JsonProperty("o")]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// High price
    /// </summary>
    [JsonProperty("h")]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// Low price
    /// </summary>
    [JsonProperty("l")]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// Raw <c>v</c> field. This is base asset volume for USDⓈ-M events and contract volume for COIN-M events.
    /// Use <see cref="BaseAssetVolume"/> or <see cref="ContractVolume"/> for product-safe access.
    /// </summary>
    [JsonProperty("v")]
    public decimal RawVolume { get; set; }

    /// <summary>
    /// Raw <c>q</c> field. This is quote asset volume for USDⓈ-M events and base asset volume for COIN-M events.
    /// Use <see cref="BaseAssetVolume"/> or <see cref="QuoteAssetVolume"/> for product-safe access.
    /// </summary>
    [JsonProperty("q")]
    public decimal RawQuoteOrBaseAssetVolume { get; set; }

    /// <summary>
    /// Symbol type after UM/CM integration: 1 = USDⓈ-M, 2 = COIN-M
    /// </summary>
    [JsonProperty("st")]
    public int SymbolType { get; set; }

    /// <summary>
    /// Total traded base asset volume, or <see langword="null"/> for an unknown symbol type
    /// </summary>
    [JsonIgnore]
    public decimal? BaseAssetVolume => SymbolType switch
    {
        1 => RawVolume,
        2 => RawQuoteOrBaseAssetVolume,
        _ => null
    };

    /// <summary>
    /// Total traded quote asset volume for USDⓈ-M events; COIN-M mini ticker events do not publish this value
    /// </summary>
    [JsonIgnore]
    public decimal? QuoteAssetVolume => SymbolType == 1 ? RawQuoteOrBaseAssetVolume : null;

    /// <summary>
    /// Total traded contract volume for COIN-M events; USDⓈ-M mini ticker events do not publish this value
    /// </summary>
    [JsonIgnore]
    public decimal? ContractVolume => SymbolType == 2 ? RawVolume : null;
}
