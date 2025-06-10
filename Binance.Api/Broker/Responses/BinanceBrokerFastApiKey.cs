namespace Binance.Api.Broker;

/// <summary>
/// Fast API User API Key
/// </summary>
public record BinanceBrokerFastApiKey
{
    /// <summary>
    /// API Key
    /// </summary>
    [JsonProperty("apiKey")]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// API Name
    /// </summary>
    [JsonProperty("apiName")]
    public string ApiName { get; set; } = string.Empty;

    /// <summary>
    /// Secret Key
    /// </summary>
    [JsonProperty("secretKey")]
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Enable Trade
    /// </summary>
    [JsonProperty("enableTrade")]
    public bool EnableTrade { get; set; }

    /// <summary>
    /// Enable Future Trade
    /// </summary>
    [JsonProperty("enableFutureTrade")]
    public bool EnableFutureTrade { get; set; }

    /// <summary>
    /// Enable Margin
    /// </summary>
    [JsonProperty("enableMargin")]
    public bool EnableMargin { get; set; }

    /// <summary>
    /// Enable European Options
    /// </summary>
    [JsonProperty("enableEuropeanOptions")]
    public bool EnableEuropeanOptions { get; set; }

    /// <summary>
    /// Create Time
    /// </summary>
    [JsonProperty("createTime")]
    public DateTime CreateTime { get; set; }
}