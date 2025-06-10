namespace Binance.Api.Broker;

/// <summary>
/// Broker -> Futures -> Customer Id &amp; Broker Id
/// </summary>
public record BinanceBrokerFuturesCustomerIdClient
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
