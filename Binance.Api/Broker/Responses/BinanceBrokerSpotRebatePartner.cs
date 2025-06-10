namespace Binance.Api.Broker;

/// <summary>
/// Broker -> Spot -> Rebate (for Partner)
/// </summary>
public record BinanceBrokerSpotRebatePartner: BinanceBrokerSpotRebateClient
{
    /// <summary>
    /// Customer Id 
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Order Id
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Trade Id
    /// </summary>
    public long TradeId { get; set; }
}
