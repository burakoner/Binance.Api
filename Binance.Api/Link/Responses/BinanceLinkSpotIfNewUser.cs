namespace Binance.Api.Link;

/// <summary>
/// If the new user info
/// </summary>
public record BinanceLinkSpotIfNewUser
{
    /// <summary>
    /// Api Agent Code
    /// </summary>
    [JsonProperty("apiAgentCode")]
    public string ApiAgentCode { get; set; } = string.Empty;

    /// <summary>
    /// If the apiAgentCode is working
    /// </summary>
    [JsonProperty("rebateWorking")]
    public bool RebateWorking { get; set; }

    /// <summary>
    /// If new user
    /// </summary>
    [JsonProperty("ifNewUser")]
    public bool IfNewUser { get; set; }

    /// <summary>
    /// Referrer Id
    /// </summary>
    [JsonProperty("referrerId")]
    public long ReferrerId { get; set; }
}
