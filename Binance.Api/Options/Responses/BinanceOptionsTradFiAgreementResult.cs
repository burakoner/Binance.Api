namespace Binance.Api.Options;

/// <summary>
/// TradFi Options agreement-signing result
/// </summary>
public record BinanceOptionsTradFiAgreementResult
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
