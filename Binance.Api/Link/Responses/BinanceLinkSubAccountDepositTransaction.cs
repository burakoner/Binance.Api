using Binance.Api.SubAccount;

namespace Binance.Api.Link;

/// <summary>
/// Sub Account Deposit Transaction
/// </summary>
public record BinanceLinkSubAccountDepositTransaction
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subAccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Address
    /// </summary>
    [JsonProperty("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Address Tag
    /// </summary>
    [JsonProperty("addressTag")]
    public string AddressTag { get; set; } = string.Empty;

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Date
    /// </summary>
    [JsonProperty("insertTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Network
    /// </summary>
    [JsonProperty("network")]
    public string Network { get; set; } = string.Empty;

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceDepositStatus Status { get; set; }

    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("txId")]
    public string TransactionId { get; set; } = string.Empty;

    /// <summary>
    /// Source Address
    /// </summary>
    [JsonProperty("sourceAddress")]
    public string SourceAddress { get; set; } = string.Empty;

    /// <summary>
    /// Confirm Times
    /// </summary>
    [JsonProperty("confirmTimes")]
    public string ConfirmTimes { get; set; } = string.Empty;
}