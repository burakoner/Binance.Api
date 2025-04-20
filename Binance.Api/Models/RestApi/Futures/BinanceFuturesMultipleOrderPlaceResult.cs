namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// Extension to be able to deserialize an error response as well
/// </summary>
internal record BinanceFuturesMultipleOrderPlaceResult : BinanceFuturesPlacedOrder
{
    public int Code { get; set; }
    [JsonProperty("msg")]
    public string? Message { get; set; }
}
