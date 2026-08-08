namespace Binance.Api.Margin;

/// <summary>Margin account scope to liquidate manually.</summary>
public enum BinanceMarginLiquidationType : byte
{
    /// <summary>Cross Margin account.</summary>
    [Map("MARGIN")]
    CrossMargin = 1,

    /// <summary>Isolated Margin account.</summary>
    [Map("ISOLATED")]
    IsolatedMargin = 2
}
