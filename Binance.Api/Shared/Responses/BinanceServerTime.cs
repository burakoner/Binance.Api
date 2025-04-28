namespace Binance.Api.Shared;

internal record BinanceServerTime
{
    [JsonProperty("serverTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ServerTime { get; set; }
}