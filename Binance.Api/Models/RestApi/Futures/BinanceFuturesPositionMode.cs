namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// User's position mode
/// </summary>
public record BinanceFuturesPositionMode
{
    /// <summary>
    /// true": Hedge Mode mode; "false": One-way Mode
    /// </summary>
    [JsonProperty("dualSidePosition"), JsonConverter(typeof(PositionModeConverter))]
    public PositionMode PositionMode { get; set; }
}
