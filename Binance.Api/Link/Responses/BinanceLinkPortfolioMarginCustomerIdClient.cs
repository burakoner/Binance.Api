namespace Binance.Api.Link;

/// <summary>
/// Link -> Portfolio Margin -> Customer Id &amp; Broker Id
/// </summary>
public record BinanceLinkPortfolioMarginCustomerIdClient
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
