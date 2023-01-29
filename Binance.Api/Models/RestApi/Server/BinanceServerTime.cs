namespace Binance.ApiClient.Models.RestApi.Server;

public class BinanceServerTime
{
    [JsonProperty("serverTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ServerTime { get; set; }
}