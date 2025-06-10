namespace Binance.Api.Broker;

/// <summary>
/// Broker -> Spot -> Customer Id
/// </summary>
public record BinanceBrokerSpotCustomerIdClient
{
    /// <summary>
    /// CustomerId 
    /// </summary>
    [JsonProperty("customerId")]
    public string CustomerId { get; set; } = string.Empty;
}
