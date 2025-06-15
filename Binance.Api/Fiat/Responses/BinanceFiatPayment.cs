namespace Binance.Api.Fiat;

/// <summary>
/// Binance Fiat Payment Record
/// </summary>
public record BinanceFiatPayment
{
    /// <summary>
    /// Order Number
    /// </summary>
    [JsonProperty("orderNo")]
    public long OrderNumber { get; set; }

    /// <summary>
    /// Source Quantity
    /// </summary>
    [JsonProperty("sourceAmount")]
    public decimal SourceQuantity { get; set; }

    /// <summary>
    /// Fiat Currency
    /// </summary>
    [JsonProperty("fiatCurrency")]
    public string FiatCurrency { get; set; } = "";

    /// <summary>
    /// Obtain Quantity
    /// </summary>
    [JsonProperty("obtainAmount")]
    public decimal ObtainQuantity { get; set; }

    /// <summary>
    /// Crypto Currency
    /// </summary>
    [JsonProperty("cryptoCurrency")]
    public string CryptoCurrency { get; set; } = "";

    /// <summary>
    /// Total Fee
    /// </summary>
    [JsonProperty("totalFee")]
    public decimal Fee { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = "";

    /// <summary>
    /// Method
    /// </summary>
    [JsonProperty("paymentMethod")]
    public string Method { get; set; } = "";

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