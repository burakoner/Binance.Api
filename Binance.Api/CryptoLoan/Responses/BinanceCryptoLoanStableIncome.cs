namespace Binance.Api.CryptoLoan;

/// <summary>
/// Crypto loan income info
/// </summary>
public record BinanceCryptoLoanStableIncome
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Income type
    /// </summary>
    [JsonProperty("type")]
    public BinanceCryptoLoanStableIncomeType Type { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("tranId")]
    public string TransactionId { get; set; } = string.Empty;
}
