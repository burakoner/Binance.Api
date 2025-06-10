namespace Binance.Api.Broker;

/// <summary>
/// Broker -> Spot -> Customer Id &amp; Email
/// </summary>
public record BinanceBrokerSpotCustomerIdPartner
{
    /// <summary>
    /// Email 
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// CustomerId 
    /// </summary>
    [JsonProperty("customerId")]
    public string CustomerId { get; set; } = string.Empty;
}
