namespace Binance.Api.Link;

/// <summary>
/// Add IP Restriction Result
/// </summary>
public record BinanceLinkAddIpRestrictionResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Api key
    /// </summary>
    [JsonProperty("apikey")]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// IP
    /// </summary>
    [JsonProperty("ip")]
    public string Ip { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateDate { get; set; }
}