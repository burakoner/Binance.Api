namespace Binance.Api.Futures;

/// <summary>
/// Futures 24-hour ticker update
/// </summary>
public record BinanceFuturesStreamTick : BinanceFuturesStreamEvent
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
    /// Price change
    /// </summary>
    [JsonProperty("p")]
    public decimal PriceChange { get; set; }

    /// <summary>
    /// Price change percentage
    /// </summary>
    [JsonProperty("P")]
    public decimal PriceChangePercent { get; set; }

    /// <summary>
    /// Weighted average price
    /// </summary>
    [JsonProperty("w")]
    public decimal WeightedAveragePrice { get; set; }

    /// <summary>
    /// Last price
    /// </summary>
    [JsonProperty("c")]
    public decimal LastPrice { get; set; }

    /// <summary>
    /// Last quantity
    /// </summary>
    [JsonProperty("Q")]
    public decimal LastQuantity { get; set; }

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
    /// Statistics open time
    /// </summary>
    [JsonProperty("O"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// Statistics close time
    /// </summary>
    [JsonProperty("C"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }

    /// <summary>
    /// First trade ID
    /// </summary>
    [JsonProperty("F")]
    public long FirstTradeId { get; set; }

    /// <summary>
    /// Last trade ID
    /// </summary>
    [JsonProperty("L")]
    public long LastTradeId { get; set; }

    /// <summary>
    /// Total number of trades
    /// </summary>
    [JsonProperty("n")]
    public long TotalTrades { get; set; }

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
    /// Total traded quote asset volume for USDⓈ-M events; COIN-M ticker events do not publish this value
    /// </summary>
    [JsonIgnore]
    public decimal? QuoteAssetVolume => SymbolType == 1 ? RawQuoteOrBaseAssetVolume : null;

    /// <summary>
    /// Total traded contract volume for COIN-M events; USDⓈ-M ticker events do not publish this value
    /// </summary>
    [JsonIgnore]
    public decimal? ContractVolume => SymbolType == 2 ? RawVolume : null;
}
