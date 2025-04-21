namespace Binance.Api.Wallet;

/// <summary>
/// Information about a withdrawal
/// </summary>
public record BinanceWithdrawal
{
    /// <summary>
    /// The id of the withdrawal
    /// </summary>
    public string Id { get; set; } = "";

    /// <summary>
    /// Withdraw order id
    /// </summary>
    public string WithdrawOrderId { get; set; } = "";

    /// <summary>
    /// The time the withdrawal was applied for
    /// </summary>
    public DateTime ApplyTime { get; set; }

    /// <summary>
    /// The quantity of the withdrawal
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// The address the asset was withdrawn to
    /// </summary>
    public string Address { get; set; } = "";

    /// <summary>
    /// Tag for the address
    /// </summary>
    public string AddressTag { get; set; } = "";

    /// <summary>
    /// The transaction id of the withdrawal
    /// </summary>
    [JsonProperty("txId")]
    public string TransactionId { get; set; } = "";

    /// <summary>
    /// Transaction fee for the withdrawal
    /// </summary>
    public decimal TransactionFee { get; set; }

    /// <summary>
    /// The asset that was withdrawn
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Network that was used
    /// </summary>
    public string Network { get; set; } = "";

    /// <summary>
    /// Confirm times for withdraw
    /// </summary>
    [JsonProperty("confirmNo")]
    public int? Confirmations { get; set; }

    /// <summary>
    /// The status of the withdrawal
    /// </summary>
    public BinanceWithdrawalStatus Status { get; set; }

    /// <summary>
    /// Transfer type: 1 for internal transfer, 0 for external transfer 
    /// </summary>
    public BinanceWithdrawDepositTransferType TransferType { get; set; }

    /// <summary>
    /// Transaction key
    /// </summary>
    [JsonProperty("txKey")]
    public string TransactionKey { get; set; } = string.Empty;

    /// <summary>
    /// Info
    /// </summary>
    public string Info { get; set; } = string.Empty;

    /// <summary>
    /// The wallet type the withdrawal was from
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceWalletType WalletType { get; set; }

    /// <summary>
    /// The time the withdrawal was completed
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? CompleteTime { get; set; }
}
