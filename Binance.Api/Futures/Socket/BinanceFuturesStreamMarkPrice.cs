namespace Binance.Api.Futures;

/// <summary>
/// Mark price update
/// </summary>
public record BinanceFuturesStreamMarkPrice: BinanceFuturesStreamEvent
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Mark Price
    /// </summary>
    [JsonProperty("p")]
    public decimal MarkPrice { get; set; }

    /// <summary>
    /// Estimated Settle Price, only useful in the last hour before the settlement starts
    /// </summary>
    [JsonProperty("P")]
    public decimal EstimatedSettlePrice { get; set; }

    /// <summary>
    /// Index price
    /// </summary>
    [JsonProperty("i")]
    public decimal IndexPrice { get; set; }

    /// <summary>
    /// Next Funding Rate
    /// </summary>
    [JsonProperty("r")]
    public decimal? FundingRate { get; set; }
    
    /// <summary>
    /// Next Funding Time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime NextFundingTime { get; set; }
}

/// <summary>
/// Mark price update
/// </summary>
public record BinanceFuturesUsdtStreamMarkPrice : BinanceFuturesStreamMarkPrice
{
    /// <summary>
    /// Mark price moving average
    /// </summary>
    [JsonProperty("ap")]
    public decimal MarkPriceMovingAverage { get; set; }

    /// <summary>
    /// Symbol type after UM/CM integration: 1 = UM, 2 = CM
    /// </summary>
    [JsonProperty("st")]
    public int SymbolType { get; set; }
}

/// <summary>
/// Mark price update
/// </summary>
public record BinanceFuturesCoinStreamMarkPrice : BinanceFuturesStreamMarkPrice
{
    /// <summary>
    /// Symbol type after UM/CM integration: 1 = UM, 2 = CM
    /// </summary>
    [JsonProperty("st")]
    public int SymbolType { get; set; }
}

/// <summary>
/// Cross-host all-market mark-price update
/// </summary>
public record BinanceFuturesStreamAllMarketMarkPrice : BinanceFuturesStreamMarkPrice
{
    /// <summary>
    /// Mark price moving average
    /// </summary>
    [JsonProperty("ap")]
    public decimal MarkPriceMovingAverage { get; set; }

    /// <summary>
    /// Symbol type after UM/CM integration: 1 = UM, 2 = CM
    /// </summary>
    [JsonProperty("st")]
    public int SymbolType { get; set; }
}
