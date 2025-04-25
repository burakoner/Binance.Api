namespace Binance.Api.Shared.Responses;

internal record BinanceServerTime
{
    [JsonProperty("serverTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ServerTime { get; set; }
}