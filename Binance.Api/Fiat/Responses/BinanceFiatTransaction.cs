namespace Binance.Api.Fiat;

/// <summary>
/// Binance Fiat Transaction Record
/// </summary>
public record BinanceFiatTransaction
{
    /// <summary>
    /// Order Number
    /// </summary>
    [JsonProperty("orderNo")]
    public long OrderNumber { get; set; }

    /// <summary>
    /// Fiat Currency
    /// </summary>
    [JsonProperty("fiatCurrency")]
    public string FiatCurrency { get; set; } = "";

    /// <summary>
    /// Indicated Quantity
    /// </summary>
    [JsonProperty("indicatedAmount")]
    public decimal IndicatedQuantity { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Total Fee
    /// </summary>
    [JsonProperty("totalFee")]
    public decimal Fee { get; set; }

    /// <summary>
    /// Method
    /// </summary>
    [JsonProperty("method")]
    public string Method { get; set; } = "";

    /// <summary>
    /// Processing, Failed, Successful, Finished, Refunding, Refunded, Refund Failed, Order Partial credit Stopped, Expired
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = "";

    /// <summary>
    /// Create Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? UpdateTime { get; set; }
}