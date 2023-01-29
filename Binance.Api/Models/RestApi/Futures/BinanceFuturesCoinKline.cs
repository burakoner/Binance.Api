using Binance.ApiClient.Models.RestApi.MarketData;

namespace Binance.ApiClient.Models.RestApi.Futures;

/// <summary>
/// Candlestick information for symbol
/// </summary>
[JsonConverter(typeof(ArrayConverter))]
public class BinanceFuturesCoinKline : BinanceKlineBase
{
    /// <inheritdoc/>
    [ArrayProperty(7)]
    public override decimal Volume { get; set; }
    /// <inheritdoc/>
    [ArrayProperty(5)]
    public override decimal QuoteVolume { get; set; }
    /// <inheritdoc/>
    [ArrayProperty(10)]
    public override decimal TakerBuyBaseVolume { get; set; }
    /// <inheritdoc/>
    [ArrayProperty(9)]
    public override decimal TakerBuyQuoteVolume { get; set; }
}
