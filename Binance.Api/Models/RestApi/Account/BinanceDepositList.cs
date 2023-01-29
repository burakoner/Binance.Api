namespace Binance.ApiClient.Models.RestApi.Account;

/// <summary>
/// Information about a deposit
/// </summary>
public class BinanceDeposit
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
    public string Asset { get; set; }
    /// <summary>
    /// The address of the deposit
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// The tag of the address of the deposit
    /// </summary>
    public string AddressTag { get; set; }
    /// <summary>
    /// The network
    /// </summary>
    public string Network { get; set; }
    /// <summary>
    /// The transaction id
    /// </summary>
    [JsonProperty("txId")]
    public string Id { get; set; }
    /// <summary>
    /// The status of the deposit
    /// </summary>
    [JsonConverter(typeof(DepositStatusConverter))]
    public DepositStatus Status { get; set; }

    /// <summary>
    /// The transfer type
    /// </summary>
    [JsonConverter(typeof(WithdrawDepositTransferTypeConverter))]
    public WithdrawDepositTransferType TransferType { get; set; }

    /// <summary>
    /// Confirmations
    /// </summary>
    [JsonProperty("confirmTimes")]
    public string Confirmations { get; set; }
    /// <summary>
    /// Network confirmations for unlocking
    /// </summary>
    [JsonProperty("unlockConfirm")]
    public string ConfirmationsForUnlock { get; set; }
}
