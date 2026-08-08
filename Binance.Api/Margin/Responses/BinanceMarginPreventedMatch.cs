namespace Binance.Api.Margin;

/// <summary>
/// A Margin match prevented by self-trade prevention.
/// </summary>
public record BinanceMarginPreventedMatch
{
    /// <summary>
    /// Trading symbol of the taker order.
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Prevented-match identifier.
    /// </summary>
    public long PreventedMatchId { get; set; }

    /// <summary>
    /// Taker order identifier.
    /// </summary>
    public long TakerOrderId { get; set; }

    /// <summary>
    /// Trading symbol of the maker order.
    /// </summary>
    public string MakerSymbol { get; set; } = "";

    /// <summary>
    /// Maker order identifier.
    /// </summary>
    public long MakerOrderId { get; set; }

    /// <summary>
    /// Trade-group identifier.
    /// </summary>
    public long TradeGroupId { get; set; }

    /// <summary>
    /// Self-trade-prevention mode that prevented the match.
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceSelfTradePreventionMode SelfTradePreventionMode { get; set; }

    /// <summary>
    /// Match price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Maker quantity prevented from matching.
    /// </summary>
    public decimal MakerPreventedQuantity { get; set; }

    /// <summary>
    /// Transaction time.
    /// </summary>
    [JsonProperty("transactTime")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }
}
