namespace Binance.Api.Broker;

/// <summary>
/// Broker -> Spot -> Rebate (for Client)
/// </summary>
public record BinanceBrokerSpotRebateClient
{
    /// <summary>
    /// Income
    /// </summary>
    public decimal Income { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Time
    /// </summary>
    public DateTime Time { get; set; }
}
