namespace Binance.Api.Mining;

/// <summary>
/// Mining account
/// </summary>
public record BinanceMiningAccount
{
    /// <summary>
    /// Type
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// User name
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Hash rates
    /// </summary>
    [JsonProperty("list")]
    public List<BinanceMiningHashrate> Hashrates { get; set; } = [];
}
