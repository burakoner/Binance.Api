namespace Binance.Api.Broker;

/// <summary>
/// Enable Leverage Token Result
/// </summary>
public record BinanceBrokerEnableLeverageTokenResult
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