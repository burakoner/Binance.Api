namespace Binance.Api.Models.RestApi.CryptoLoans;

/// <summary>
/// Crypto loan income info
/// </summary>
public class BinanceCryptoLoanIncome
{
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; }
    /// <summary>
    /// Income type
    /// </summary>
    [JsonConverter(typeof(LoanIncomeTypeConverter))]
    public LoanIncomeType Type { get; set; }
    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("tranId")]
    public string TransactionId { get; set; }
}
