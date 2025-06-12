namespace Binance.Api.Options;

/// <summary>
/// Binance Options Trade
/// </summary>
public record BinanceOptionsPublicTrade
{
    /// <summary>
    /// The id of the trade
    /// </summary>
    [JsonProperty("id")]
    public long TradeId { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The price of the trade
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The base quantity of the trade
    /// </summary>
    [JsonProperty("qty")]
    public decimal BaseQuantity { get; set; }

    /// <summary>
    /// The quote quantity of the trade
    /// </summary>
    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }

    /// <summary>
    /// Completed trade direction（-1 Sell，1 Buy）
    /// </summary>
    public BinanceOptionsTradeSide Side { get; set; }

    /// <summary>
    /// The timestamp of the trade
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }
}