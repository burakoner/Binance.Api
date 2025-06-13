namespace Binance.Api.Link;

/// <summary>
/// Fast API User Status
/// </summary>
public record BinanceLinkFastUserStatus
{
    /// <summary>
    /// Is Exist Future Account?
    /// </summary>
    [JsonProperty("isExistFutureAccount")]
    public bool IsExistFutureAccount { get; set; }
}