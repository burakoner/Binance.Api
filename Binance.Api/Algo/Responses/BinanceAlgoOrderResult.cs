namespace Binance.Api.Algo;

/// <summary>
/// Algo order result
/// </summary>
public record BinanceAlgoOrderResult
{
    /// <summary>
    /// Result code
    /// </summary>
    [JsonProperty("code")]
    public long Code { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    [JsonProperty("msg")]
    public string? Message { get; set; }

    /// <summary>
    /// Successful
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Order id
    /// </summary>
    public string ClientAlgoId { get; set; } = string.Empty;

    /// <summary>
    /// The order id as assigned by the client without the prefix
    /// </summary>
    public string RequestClientAlgoId => BinanceHelpers.RemoveBrokerId(ClientAlgoId);
}
