namespace Binance.Api.DualInvestment;

/// <summary>
/// Binance Dual Investment Account
/// </summary>
public record BinanceDualInvestmentAccount
{
    /// <summary>
    /// Total BTC amounts in Dual Investment
    /// </summary>
    [JsonProperty("totalAmountInBTC")]
    public decimal TotalQuantityInBTC { get; set; }

    /// <summary>
    /// Total USDT equivalents in BTC in Dual Investment
    /// </summary>
    [JsonProperty("totalAmountInUSDT")]
    public decimal TotalQuantityInUSDT { get; set; }
}