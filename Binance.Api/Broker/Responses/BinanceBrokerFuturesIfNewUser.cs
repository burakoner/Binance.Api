namespace Binance.Api.Broker;

/// <summary>
/// If the new user info
/// </summary>
public record BinanceBrokerFuturesIfNewUser
{
    /// <summary>
    /// Broker Id
    /// </summary>
    [JsonProperty("brokerId")]
    public string BrokerId { get; set; } = string.Empty;

    /// <summary>
    /// If the apiAgentCode is working
    /// </summary>
    [JsonProperty("rebateWorking")]
    public bool RebateWorking { get; set; }

    /// <summary>
    /// If new user
    /// </summary>
    [JsonProperty("ifNewUser")]
    public bool IfNewUser { get; set; }
}
