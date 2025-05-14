namespace Binance.Api.SubAccount;

/// <summary>
/// Binance sub account universal transaction
/// </summary>
public record BinanceSubAccountUniversalTransfer
{
    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// From email
    /// </summary>
    [JsonProperty("fromEmail")]
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// To email
    /// </summary>
    [JsonProperty("toEmail")]
    public string ToEmail { get; set; } = string.Empty;

    /// <summary>
    /// From account type
    /// </summary>
    [JsonProperty("fromAccountType")]
    public BinanceSubAccountTransferType FromAccountType { get; set; }

    /// <summary>
    /// To account type
    /// </summary>
    [JsonProperty("toAccountType")]
    public BinanceSubAccountTransferType ToAccountType { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// The time the universal transaction was created
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("createTimeStamp")]
    public DateTime CreateTime { get; set; }
}
