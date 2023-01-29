namespace Binance.ApiClient.Models.RestApi.MarketData;

/// <summary>
/// Book price
/// </summary>
public class BinanceFuturesBookPrice : BinanceBookPrice
{
    /// <summary>
    /// Pair
    /// </summary>
    public string Pair { get; set; }
}
