namespace Binance.Api.Futures;

/// <summary>
/// User's position mode
/// </summary>
public record BinanceFuturesPositionMode
{
    /// <summary>
    /// true": Hedge Mode mode; "false": One-way Mode
    /// </summary>
    [JsonProperty("dualSidePosition")]
    public bool IsHedgeMode { get; set; }
}
