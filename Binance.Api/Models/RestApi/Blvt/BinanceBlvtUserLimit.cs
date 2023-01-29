namespace Binance.ApiClient.Models.RestApi.Blvt;

/// <summary>
/// Leveraged tokens user limits
/// </summary>
public class BinanceBlvtUserLimit
{
    /// <summary>
    /// Token name
    /// </summary>
    public string TokenName { get; set; }
    /// <summary>
    /// Daily purchase limit
    /// </summary>
    public decimal UserDailyTotalPurchaseLimit { get; set; }
    /// <summary>
    /// Daily redeem limit
    /// </summary>
    public decimal UserDailyTotalRedeemLimit { get; set; }
}
