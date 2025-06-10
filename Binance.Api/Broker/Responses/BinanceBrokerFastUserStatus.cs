namespace Binance.Api.Broker;

/// <summary>
/// Fast API User Status
/// </summary>
public record BinanceBrokerFastUserStatus
{
    /// <summary>
    /// Is Exist Future Account?
    /// </summary>
    [JsonProperty("isExistFutureAccount")]
    public bool IsExistFutureAccount { get; set; }
}