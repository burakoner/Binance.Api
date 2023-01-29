namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// Network for an asset
/// </summary>
public class BinanceNetwork
{
    /// <summary>
    /// Regex for an address on the network
    /// </summary>
    public string AddressRegex { get; set; }
    /// <summary>
    /// Asset name
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; }
    /// <summary>
    /// Deposit description
    /// </summary>
    [JsonProperty("depositDesc")]
    public string DepositDescription { get; set; }
    /// <summary>
    /// Deposit enabled
    /// </summary>
    [JsonProperty("depositEnable")]
    public bool DepositEnabled { get; set; }
    /// <summary>
    /// Is default network
    /// </summary>
    public bool IsDefault { get; set; }
    /// <summary>
    /// Regex for a memo
    /// </summary>
    public string MemoRegex { get; set; }
    /// <summary>
    /// Minimal confirmations for balance confirmation
    /// </summary>
    [JsonProperty("minConfirm")]
    public int MinConfirmations { get; set; }
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Network
    /// </summary>
    public string Network { get; set; }
    /// <summary>
    /// Reset address status
    /// </summary>
    public bool ResetAddressStatus { get; set; }
    /// <summary>
    /// Tips
    /// </summary>
    public string SpecialTips { get; set; }
    /// <summary>
    /// Confirmation number for balance unlock
    /// </summary>
    public int UnlockConfirm { get; set; }

    /// <summary>
    /// Withdraw description
    /// </summary>
    [JsonProperty("withdrawDesc")]
    public string WithdrawDescription { get; set; }
    /// <summary>
    /// Withdraw is enabled
    /// </summary>
    [JsonProperty("withdrawEnable")]
    public bool WithdrawEnabled { get; set; }
    /// <summary>
    /// Fee for withdrawing
    /// </summary>
    public decimal WithdrawFee { get; set; }
    /// <summary>
    /// Minimal withdraw quantity
    /// </summary>
    public decimal WithdrawMin { get; set; }
    /// <summary>
    /// Min withdraw step
    /// </summary>
    public decimal WithdrawIntegerMultiple { get; set; }
    /// <summary>
    /// Max withdraw quantity
    /// </summary>
    public decimal WithdrawMax { get; set; }
    /// <summary>
    /// If the asset needs to provide memo to withdraw
    /// </summary>
    public bool SameAddress { get; set; }
}
