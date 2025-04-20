namespace Binance.Api.Models.RestApi.Account;

public record BinanceIpRestrictionUpdate
{
    /// <summary>
    /// Ip Restriction Status
    /// </summary>
    [JsonConverter(typeof(IpRestrictionStatusConverter))]
    public IpRestrictionStatus Status { get; set; }

    /// <summary>
    /// Ip Whitelist
    /// </summary>
    public IEnumerable<string> IpList { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// The API key
    /// </summary>
    public string ApiKey { get; set; } = "";
}
