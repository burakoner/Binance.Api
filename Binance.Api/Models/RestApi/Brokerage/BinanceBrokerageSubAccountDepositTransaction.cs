namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Sub Account Deposit Transaction
/// </summary>
public record BinanceBrokerageSubAccountDepositTransaction
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; } = "";

    /// <summary>
    /// Address
    /// </summary>
    public string Address { get; set; } = "";

    /// <summary>
    /// Address Tag
    /// </summary>
    public string AddressTag { get; set; } = "";

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Date
    /// </summary>
    [JsonProperty("insertTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Network
    /// </summary>
    public string Network { get; set; } = "";

    /// <summary>
    /// Status
    /// </summary>
    public BinanceBrokerageSubAccountDepositStatus Status { get; set; }

    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("txId")]
    public string TransactionId { get; set; } = "";

    /// <summary>
    /// Source Address
    /// </summary>
    public string SourceAddress { get; set; } = "";

    /// <summary>
    /// Confirm Times
    /// </summary>
    public string ConfirmTimes { get; set; } = "";
}