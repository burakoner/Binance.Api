namespace Binance.Api.Futures;

/// <summary>
/// USD-M TradFi Perps agreement-signing result
/// </summary>
public record BinanceFuturesTradFiPerpsAgreementResult
{
    /// <summary>
    /// Result code
    /// </summary>
    [JsonProperty("code")]
    public long Code { get; set; }

    /// <summary>
    /// Result message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = string.Empty;
}
