namespace Binance.Api.Spot;

internal sealed class BinanceSpotUserDataStreamRequest : BinanceSocketQuery
{
    [JsonIgnore]
    public decimal? ReceiveWindow { get; set; }

    [JsonIgnore]
    public int? SubscriptionId { get; set; }
}
