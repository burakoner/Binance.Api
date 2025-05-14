namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account historic transfer
/// </summary>
public record BinanceSubAccountTransferSubAccount
{
    /// <summary>
    /// Counter party of the transfer
    /// </summary>
    [JsonProperty("counterParty")]
    public string CounterParty { get; set; } = string.Empty;

    /// <summary>
    /// Email of the account
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// From account type
    /// </summary>
    [JsonProperty("fromAccountType")]
    public string FromAccountType { get; set; } = string.Empty;

    /// <summary>
    /// To account type
    /// </summary>
    [JsonProperty("toAccountType")]
    public string ToAccountType { get; set; } = string.Empty;

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Transfer type
    /// </summary>
    [JsonProperty("type")]
    public BinanceSubAccountTransferType Type { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Timestamp of the transfer
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}
