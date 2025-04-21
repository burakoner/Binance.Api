namespace Binance.Api.Wallet;

/// <summary>
/// Information about a deposit
/// </summary>
public record BinanceDeposit
{
    /// <summary>
    /// Time the deposit was added to Binance
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
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
    public string Asset { get; set; } = "";

    /// <summary>
    /// The address of the deposit
    /// </summary>
    public string Address { get; set; } = "";

    /// <summary>
    /// The tag of the address of the deposit
    /// </summary>
    public string AddressTag { get; set; } = "";

    /// <summary>
    /// The network
    /// </summary>
    public string Network { get; set; } = "";

    /// <summary>
    /// The transaction id
    /// </summary>
    [JsonProperty("txId")]
    public string Id { get; set; } = "";

    /// <summary>
    /// The transaction id
    /// </summary>
    [JsonProperty("txId")]
    public string TransactionId { get; set; } = string.Empty;

    /// <summary>
    /// The status of the deposit
    /// </summary>
    public BinanceDepositStatus Status { get; set; }

    /// <summary>
    /// The transfer type
    /// </summary>
    public BinanceWithdrawDepositTransferType TransferType { get; set; }

    /// <summary>
    /// Confirmations
    /// </summary>
    [JsonProperty("confirmTimes")]
    public string Confirmations { get; set; } = "";

    /// <summary>
    /// Network confirmations for unlocking
    /// </summary>
    [JsonProperty("unlockConfirm")]
    public string ConfirmationsForUnlock { get; set; } = "";

    /// <summary>
    /// The wallet type
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceWalletType WalletType { get; set; }

    /// <summary>
    /// Transaction source address. Note: Please note that the source address returned may not be accurate due to network-specific characteristics. If multiple source addresses found, only the first address will be returned
    /// </summary>
    public string? SourceAddress { get; set; }
}
