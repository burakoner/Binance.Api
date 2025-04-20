namespace Binance.Api.Models.RestApi.MarketData;

/// <summary>
/// Book price
/// </summary>
public record BinanceFuturesBookPrice : BinanceBookPrice
{
    /// <summary>
    /// Pair
    /// </summary>
    public string Pair { get; set; } = "";
}
