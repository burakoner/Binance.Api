namespace Binance.Api.Link;

/// <summary>
/// Enable Leverage Token Result
/// </summary>
public record BinanceLinkEnableLeverageTokenResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Is Leverage Token Enabled
    /// </summary>
    [JsonProperty("enableBlvt")]
    public bool IsLeverageTokenEnabled { get; set; }
}