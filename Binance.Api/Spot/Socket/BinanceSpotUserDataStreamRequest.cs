namespace Binance.Api.Spot;

internal sealed class BinanceSpotUserDataStreamRequest : BinanceSocketQuery
{
    [JsonIgnore]
    public decimal? ReceiveWindow { get; set; }

    [JsonIgnore]
    public long? SubscriptionId { get; set; }

    [JsonIgnore]
    public bool UsesSessionAuthentication { get; set; }
}
