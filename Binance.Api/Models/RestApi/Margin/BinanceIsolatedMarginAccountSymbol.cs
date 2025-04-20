namespace Binance.Api.Models.RestApi.Margin;

/// <summary>
/// Isolated margin account symbol
/// </summary>
public record BinanceIsolatedMarginAccountSymbol
{
    /// <summary>
    /// Base asset
    /// </summary>
    public BinanceIsolatedMarginAccountAsset BaseAsset { get; set; } = default!;

    /// <summary>
    /// Quote asset
    /// </summary>
    public BinanceIsolatedMarginAccountAsset QuoteAsset { get; set; } = default!;

    /// <summary>
    /// Symbol name
    /// </summary>
    public string Symbol { get; set; } = "";
    /// <summary>
    /// Isolated created
    /// </summary>
    public bool IsolatedCreated { get; set; }
    /// <summary>
    /// The margin level
    /// </summary>
    public decimal MarginLevel { get; set; }
    /// <summary>
    /// Margin level status
    /// </summary>
    [JsonConverter(typeof(MarginLevelStatusConverter))]
    public MarginLevelStatus MarginLevelStatus { get; set; }
    /// <summary>
    /// Margin ratio
    /// </summary>
    public decimal MarginRatio { get; set; }
    /// <summary>
    /// Index price
    /// </summary>
    public decimal IndexPrice { get; set; }
    /// <summary>
    /// Liquidate price
    /// </summary>
    public decimal LiquidatePrice { get; set; }
    /// <summary>
    /// Liquidate rate
    /// </summary>
    public decimal LiquidateRate { get; set; }
    /// <summary>
    /// If trading is enabled
    /// </summary>
    public bool TradeEnabled { get; set; }
    /// <summary>
    /// Account is enabled
    /// </summary>
    public bool Enabled { get; set; }
}
