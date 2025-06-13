namespace Binance.Api.Link;

/// <summary>
/// Link -> Futures -> Customer Id &amp; Broker Id
/// </summary>
public record BinanceLinkFuturesCustomerIdClient
{
    /// <summary>
    /// BrokerId 
    /// </summary>
    [JsonProperty("brokerId")]
    public string BrokerId { get; set; } = string.Empty;

    /// <summary>
    /// CustomerId 
    /// </summary>
    [JsonProperty("customerId")]
    public string CustomerId { get; set; } = string.Empty;
}
