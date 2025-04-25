namespace Binance.Api.Models.WebSocketApi.Futures;

/// <summary>
/// Mark price update
/// </summary>
public record BinanceFuturesStreamMarkPrice : BinanceSocketEvent, IBinanceFuturesMarkPrice
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

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
public record BinanceFuturesUsdStreamMarkPrice : BinanceFuturesStreamMarkPrice
{
    /// <summary>
    /// Mark Price
    /// </summary>
    [JsonProperty("i")]
    public decimal IndexPrice { get; set; }
}

/// <summary>
/// Mark price update
/// </summary>
public record BinanceFuturesCoinStreamMarkPrice : BinanceFuturesStreamMarkPrice
{
    /// <summary>
    /// Mark Price
    /// </summary>
    [JsonProperty("P")]
    public new decimal EstimatedSettlePrice { get; set; }
}
