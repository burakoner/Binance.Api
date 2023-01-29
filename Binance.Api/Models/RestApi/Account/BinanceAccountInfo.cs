namespace Binance.ApiClient.Models.RestApi.Account;

/// <summary>
/// Information about an account
/// </summary>
public class BinanceAccountInfo
{
    /// <summary>
    /// Fee percentage to pay when making trades
    /// </summary>
    [JsonProperty("makerCommission")]
    public decimal MakerFee { get; set; }
    /// <summary>
    /// Fee percentage to pay when taking trades
    /// </summary>
    [JsonProperty("takerCommission")]
    public decimal TakerFee { get; set; }
    /// <summary>
    /// Fee percentage to pay when buying
    /// </summary>
    [JsonProperty("buyerCommission")]
    public decimal BuyerFee { get; set; }
    /// <summary>
    /// Fee percentage to pay when selling
    /// </summary>
    [JsonProperty("sellerCommission")]
    public decimal SellerFee { get; set; }
    /// <summary>
    /// Boolean indicating if this account can trade
    /// </summary>
    public bool CanTrade { get; set; }
    /// <summary>
    /// Boolean indicating if this account can withdraw
    /// </summary>
    public bool CanWithdraw { get; set; }
    /// <summary>
    /// Boolean indicating if this account can deposit
    /// </summary>
    public bool CanDeposit { get; set; }
    /// <summary>
    /// Account is brokered
    /// </summary>
    public bool Brokered { get; set; }
    /// <summary>
    /// The time of the update
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// The type of account
    /// </summary>
    [JsonConverter(typeof(EnumConverter))]
    public AccountType AccountType { get; set; }
    /// <summary>
    /// Permissions types
    /// </summary>
    [JsonProperty(ItemConverterType = typeof(EnumConverter))]
    public IEnumerable<AccountType> Permissions { get; set; } = Array.Empty<AccountType>();
    /// <summary>
    /// List of assets with their current balances
    /// </summary>
    public IEnumerable<BinanceBalance> Balances { get; set; } = Array.Empty<BinanceBalance>();
}
