namespace Binance.Api.Margin;

/// <summary>
/// Personal margin level information
/// </summary>
public record BinanceMarginLevel
{
    /// <summary>
    /// The level at which your margin level is considered normal.
    /// </summary>
    [JsonProperty("normalBar")]
    public decimal NormalLevel { get; set; }

    /// <summary>
    /// The level at which you will be margin called (asked to deposit more funds)
    /// </summary>
    [JsonProperty("marginCallBar")]
    public decimal MarginCallLevel { get; set; }

    /// <summary>
    /// The level at which your positions will be liquidated until your account balances
    /// </summary>
    [JsonProperty("forceLiquidationBar")]
    public decimal ForcedLiquidationLevel { get; set; }
}
