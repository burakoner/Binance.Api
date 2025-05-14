namespace Binance.Api.SubAccount;

/// <summary>
/// Information about a deposit
/// </summary>
public record BinanceSubAccountDeposit
{
    /// <summary>
    /// Time the deposit was added to Binance
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("insertTime")]
    public DateTime InsertTime { get; set; }

    /// <summary>
    /// The quantity deposited
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// The asset deposited
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Network
    /// </summary>
    [JsonProperty("network")]
    public string Network { get; set; } = string.Empty;

    /// <summary>
    /// The address of the deposit
    /// </summary>
    [JsonProperty("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// The address tag
    /// </summary>
    [JsonProperty("addressTag")]
    public string AddressTag { get; set; } = string.Empty;

    /// <summary>
    /// The transaction id
    /// </summary>
    [JsonProperty("txId")]
    public string TransactionId { get; set; } = string.Empty;

    /// <summary>
    /// Confirmation status
    /// </summary>
    [JsonProperty("confirmTimes")]
    public string ConfirmTimes { get; set; } = string.Empty;

    /// <summary>
    /// Transfer type
    /// </summary>
    [JsonProperty("transferType")]
    public int TransferType { get; set; }

    /// <summary>
    /// The status of the deposit
    /// </summary>
    [JsonProperty("status")]
    public BinanceSubAccountDepositStatus Status { get; set; }
}
