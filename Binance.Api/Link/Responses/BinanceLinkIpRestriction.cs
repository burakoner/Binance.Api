namespace Binance.Api.Link;

/// <summary>
/// IP Restriction
/// </summary>
public record BinanceLinkIpRestrictionBase
{
    /// <summary>
    /// Api key
    /// </summary>
    [JsonProperty("apikey")]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// IP list
    /// </summary>
    [JsonProperty("ipList")]
    public List<string> IpList { get; set; } = [];

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateDate { get; set; }
}

/// <summary>
/// IP Restriction
/// </summary>
public record BinanceLinkIpRestriction : BinanceLinkIpRestrictionBase
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Ip Restrict
    /// </summary>
    [JsonProperty("ipRestrict")]
    public bool IpRestrict { get; set; }
}

/// <summary>
/// IP Restriction V2
/// </summary>
public record BinanceLinkIpRestrictionV2 : BinanceLinkIpRestrictionBase
{
    /// <summary>
    /// IP Restriction Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceLinkApiKeyIpRestriction Status { get; set; }
}
