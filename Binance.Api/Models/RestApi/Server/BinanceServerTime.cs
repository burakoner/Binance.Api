namespace Binance.Api.Models.RestApi.Server;

public record BinanceServerTime
{
    [JsonProperty("serverTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ServerTime { get; set; }
}