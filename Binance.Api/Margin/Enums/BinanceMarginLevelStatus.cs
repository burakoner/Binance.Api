namespace Binance.Api.Margin;

/// <summary>
/// Margin level status
/// </summary>
public enum BinanceMarginLevelStatus : byte
{
    /// <summary>
    /// Excessive
    /// </summary>
    [Map("EXCESSIVE")]
    Excessive = 1,

    /// <summary>
    /// Normal
    /// </summary>
    [Map("NORMAL")]
    Normal,

    /// <summary>
    /// Margin call
    /// </summary>
    [Map("MARGIN_CALL")]
    MarginCall,

    /// <summary>
    /// Pre-liquidation
    /// </summary>
    [Map("PRE_LIQUIDATION")]
    PreLiquidation,

    /// <summary>
    /// Force liquidation
    /// </summary>
    [Map("FORCE_LIQUIDATION")]
    ForceLiquidation
}
