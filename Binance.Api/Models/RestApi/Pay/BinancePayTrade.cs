namespace Binance.ApiClient.Models.RestApi.Pay;

/// <summary>
/// Binance pay trade
/// </summary>
public class BinancePayTrade
{
    /// <summary>
    /// Order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter))]
    public PayOrderType OrderType { get; set; }
    /// <summary>
    /// Transaction id
    /// </summary>
    public string TransactionId { get; set; }
    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }
    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("currency")]
    public string Asset { get; set; }
    /// <summary>
    /// Fund details
    /// </summary>
    [JsonProperty("fundsDetail")]
    public IEnumerable<BinancePayTradeDetails> Details { get; set; } = Array.Empty<BinancePayTradeDetails>();
}

/// <summary>
/// Pay trade funds details
/// </summary>
public class BinancePayTradeDetails
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("currency")]
    public string Asset { get; set; }
    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
}
