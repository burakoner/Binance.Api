namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// Information about an asset for a user
/// </summary>
public record BinanceUserAsset
{
    /// <summary>
    /// Asset code
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = "";
    /// <summary>
    /// Deposit all is enabled
    /// </summary>
    public bool DepositAllEnable { get; set; }
    /// <summary>
    /// Quantity free
    /// </summary>
    [JsonProperty("free")]
    public decimal Available { get; set; }
    /// <summary>
    /// Quantity frozen
    /// </summary>
    public decimal Freeze { get; set; }
    /// <summary>
    /// Ipo-able
    /// </summary>
    public decimal Ipoable { get; set; }
    /// <summary>
    /// Ipo-ing
    /// </summary>
    public decimal Ipoing { get; set; }
    /// <summary>
    /// Is the asset legally money
    /// </summary>
    public bool IsLegalMoney { get; set; }
    /// <summary>
    /// Quantity locked
    /// </summary>
    public decimal Locked { get; set; }
    /// <summary>
    /// Storage
    /// </summary>
    public decimal Storage { get; set; }
    /// <summary>
    /// Is trading
    /// </summary>
    public bool Trading { get; set; }
    /// <summary>
    /// Withdraw all enabled
    /// </summary>
    public bool WithdrawAllEnable { get; set; }
    /// <summary>
    /// Name of the asset
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// Currently withdrawing
    /// </summary>
    public decimal Withdrawing { get; set; }
    /// <summary>
    /// Networks
    /// </summary>
    public IEnumerable<BinanceNetwork> NetworkList { get; set; } = [];
}