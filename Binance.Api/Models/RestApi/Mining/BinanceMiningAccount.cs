namespace Binance.Api.Models.RestApi.Mining;

/// <summary>
/// Mining account
/// </summary>
public class BinanceMiningAccount
{
    /// <summary>
    /// Type
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// User name
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// Hash rates
    /// </summary>
    [JsonProperty("list")]
    public IEnumerable<BinanceHashRate> Hashrates { get; set; } = Array.Empty<BinanceHashRate>();
}
