namespace Binance.ApiClient.Models.RestApi.Futures;

/// <summary>
/// Result from a change margin type request
/// </summary>
public class BinanceFuturesChangeMarginTypeResult
{
    /// <summary>
    /// Response code
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// Response message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; }
}
