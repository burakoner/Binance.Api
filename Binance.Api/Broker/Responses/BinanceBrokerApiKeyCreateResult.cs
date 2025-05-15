namespace Binance.Api.Broker;

/// <summary>
/// Api Key Create Result
/// </summary>
public record BinanceBrokerApiKeyCreateResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Api Key
    /// </summary>
    [JsonProperty("apiKey")]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Api Secret
    /// </summary>
    [JsonProperty("secretKey")]
    public string ApiSecret { get; set; } = string.Empty;

    /// <summary>
    /// Is Spot Trading Enabled
    /// </summary>
    [JsonProperty("canTrade")]
    public bool IsSpotTradingEnabled { get; set; }

    /// <summary>
    /// Is Margin Trading Enabled
    /// </summary>
    [JsonProperty("marginTrade")]
    public bool IsMarginTradingEnabled { get; set; }

    /// <summary>
    /// Is Futures Trading Enabled
    /// </summary>
    [JsonProperty("futuresTrade")]
    public bool IsFuturesTradingEnabled { get; set; }
}