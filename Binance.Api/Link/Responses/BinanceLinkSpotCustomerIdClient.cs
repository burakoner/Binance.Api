namespace Binance.Api.Link;

/// <summary>
/// Link -> Spot -> Customer Id
/// </summary>
public record BinanceLinkSpotCustomerIdClient
{
    /// <summary>
    /// CustomerId 
    /// </summary>
    [JsonProperty("customerId")]
    public string CustomerId { get; set; } = string.Empty;
}
