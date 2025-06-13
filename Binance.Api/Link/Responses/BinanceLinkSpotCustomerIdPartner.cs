namespace Binance.Api.Link;

/// <summary>
/// Link -> Spot -> Customer Id &amp; Email
/// </summary>
public record BinanceLinkSpotCustomerIdPartner
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
