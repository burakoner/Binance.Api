namespace Binance.Api.Futures;

/// <summary>
/// COIN-M futures income history result
/// </summary>
public record BinanceFuturesCoinIncomeHistory
{
    /// <summary>
    /// Trading symbol, or an empty string when the income is not associated with a symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Type of income
    /// </summary>
    [JsonProperty("incomeType")]
    [JsonConverter(typeof(MapConverter))]
    public BinanceFuturesCoinIncomeType? IncomeType { get; set; }

    /// <summary>
    /// Income amount
    /// </summary>
    [JsonProperty("income")]
    public decimal Income { get; set; }

    /// <summary>
    /// Income asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Extra information
    /// </summary>
    [JsonProperty("info")]
    public string Info { get; set; } = "";

    /// <summary>
    /// Income time
    /// </summary>
    [JsonProperty("time")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Transaction identifier, unique within the same income type for a user
    /// </summary>
    [JsonProperty("tranId")]
    public string TransactionId { get; set; } = "";

    /// <summary>
    /// Trade identifier
    /// </summary>
    [JsonProperty("tradeId")]
    public string TradeId { get; set; } = "";
}
