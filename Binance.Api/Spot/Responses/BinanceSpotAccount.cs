namespace Binance.Api.Spot;

/// <summary>
/// Information about an account
/// </summary>
public record BinanceSpotAccount
{
    /// <summary>
    /// Legacy maker commission value in basis points.
    /// </summary>
    [JsonProperty("makerCommission")]
    public long MakerCommission { get; set; }

    /// <summary>
    /// Legacy taker commission value in basis points.
    /// </summary>
    [JsonProperty("takerCommission")]
    public long TakerCommission { get; set; }

    /// <summary>
    /// Legacy buyer commission value in basis points.
    /// </summary>
    [JsonProperty("buyerCommission")]
    public long BuyerCommission { get; set; }

    /// <summary>
    /// Legacy seller commission value in basis points.
    /// </summary>
    [JsonProperty("sellerCommission")]
    public long SellerCommission { get; set; }

    /// <summary>
    /// Commission rates for this account
    /// </summary>
    [JsonProperty("commissionRates")]
    public BinanceSpotAccountFeeRates FeeRates { get; set; } = default!;

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
    /// Require self trade prevention
    /// </summary>
    public bool RequireSelfTradePrevention { get; set; }

    /// <summary>
    /// Prevent smart order routing
    /// </summary>
    [JsonProperty("preventSor")]
    public bool PreventSmartOrderRouting { get; set; }

    /// <summary>
    /// The time of the update
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// The type of account
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceAccountType AccountType { get; set; }

    /// <summary>
    /// List of assets with their current balances
    /// </summary>
    [JsonProperty("balances")]
    public List<BinanceSpotAccountBalance> Balances { get; set; } = [];

    /// <summary>
    /// Permissions types
    /// </summary>
    [JsonProperty("permissions")]
    public List<BinancePermissionType> Permissions { get; set; } = [];

    /// <summary>
    /// User id
    /// </summary>
    [JsonProperty("uid")]
    public long UserId { get; set; }
}

/// <summary>
/// Information about the fee rates for an account
/// </summary>
public record BinanceSpotAccountFeeRates
{
    /// <summary>
    /// The maker fee percentage
    /// </summary>
    public decimal Maker { get; set; }

    /// <summary>
    /// The taker fee percentage
    /// </summary>
    public decimal Taker { get; set; }

    /// <summary>
    /// The buyer fee percentage
    /// </summary>
    public decimal Buyer { get; set; }

    /// <summary>
    /// The seller fee percentage
    /// </summary>
    public decimal Seller { get; set; }
}

/// <summary>
/// Information about an asset balance
/// </summary>
public record BinanceSpotAccountBalance
{
    /// <summary>
    /// The asset this balance is for
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// The quantity that isn't locked in a trade
    /// </summary>
    [JsonProperty("free")]
    public decimal Available { get; set; }

    /// <summary>
    /// The quantity that is currently locked in a trade
    /// </summary>
    public decimal Locked { get; set; }

    /// <summary>
    /// The total balance of this asset (Free + Locked)
    /// </summary>
    public decimal Total => Available + Locked;
}
