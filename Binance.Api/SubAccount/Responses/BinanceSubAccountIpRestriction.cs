namespace Binance.Api.SubAccount;

/// <summary>
/// IP restriction info
/// </summary>
public record BinanceSubAccountIpRestriction
{
    /// <summary>
    /// Is currently restricted
    /// </summary>
    [JsonProperty("ipRestrict")]
    public bool IpRestricted { get; set; }

    /// <summary>
    /// Ip whitelist
    /// </summary>
    [JsonProperty("ipList")]
    public List<string>  IpList { get; set; } = [];
    
    /// <summary>
    /// Update Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("updateTime")]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// The API key
    /// </summary>
    [JsonProperty("apiKey")]
    public string ApiKey { get; set; } = string.Empty;
}
