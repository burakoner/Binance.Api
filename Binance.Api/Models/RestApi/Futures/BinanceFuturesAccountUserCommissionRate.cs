namespace Binance.ApiClient.Models.RestApi.Futures;

/// <summary>
/// User commission rate
/// </summary>
public class BinanceFuturesAccountUserCommissionRate
{
    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; }
    /// <summary>
    /// Maker commission rate
    /// </summary>
    public decimal MakerCommissionRate { get; set; }
    /// <summary>
    /// Taker commission rate
    /// </summary>
    public decimal TakerCommissionRate { get; set; }
}
