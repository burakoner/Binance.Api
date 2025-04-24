namespace Binance.Api.Futures;

/// <summary>
/// Extension to be able to deserialize an error response as well
/// </summary>
internal record BinanceFuturesMultipleOrderPlaceResult : BinanceFuturesOrder
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("msg")]
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Extension to be able to deserialize an error response as well
/// </summary>
internal record BinanceUsdFuturesMultipleOrderPlaceResult : BinanceUsdFuturesOrder
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("msg")]
    public string Message { get; set; } = string.Empty;
}
