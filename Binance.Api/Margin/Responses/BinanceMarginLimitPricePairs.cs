namespace Binance.Api.Margin;

/// <summary>Margin pairs subject to index-price limit-order restrictions.</summary>
public record BinanceMarginLimitPricePairs
{
    /// <summary>Restricted Cross Margin symbols.</summary>
    public List<string> CrossMarginSymbols { get; set; } = [];
}
