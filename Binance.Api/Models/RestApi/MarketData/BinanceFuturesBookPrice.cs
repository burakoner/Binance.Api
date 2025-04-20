namespace Binance.Api.Models.RestApi.MarketData;

/// <summary>
/// Book price
/// </summary>
public record BinanceFuturesBookPrice : BinanceBookTicker
{
    /// <summary>
    /// Pair
    /// </summary>
    public string Pair { get; set; } = "";
}
