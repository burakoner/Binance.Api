namespace Binance.Api.Futures;

/// <summary>
/// Futures asset index
/// </summary>
public record BinanceFuturesAssetIndex
{
    /// <summary>
    /// The symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Index
    /// </summary>
    [JsonProperty("index")]
    public decimal Index { get; set; }

    /// <summary>
    /// Bid buffer
    /// </summary>
    [JsonProperty("bidBuffer")]
    public decimal BidBuffer { get; set; }

    /// <summary>
    /// Ask buffer
    /// </summary>
    [JsonProperty("askBuffer")]
    public decimal AskBuffer { get; set; }

    /// <summary>
    /// Bid price
    /// </summary>
    [JsonProperty("bidRate")]
    public decimal BidPrice { get; set; }

    /// <summary>
    /// Ask price
    /// </summary>
    [JsonProperty("askRate")]
    public decimal AskPrice { get; set; }

    /// <summary>
    /// Auto exchange bid buffer
    /// </summary>
    [JsonProperty("autoExchangeBidBuffer")]
    public decimal AutoExchangeBidBuffer { get; set; }

    /// <summary>
    /// Auto exchange ask buffer
    /// </summary>
    [JsonProperty("autoExchangeAskBuffer")]
    public decimal AutoExchangeAskBuffer { get; set; }
    /// <summary>
    /// 
    /// Auto exchange bid price
    /// </summary>
    [JsonProperty("autoExchangeBidRate")]
    public decimal AutoExchangeBidPrice { get; set; }

    /// <summary>
    /// Auto exchange ask price
    /// </summary>
    [JsonProperty("autoExchangeAskRate")]
    public decimal AutoExchangeAskPrice { get; set; }
}
