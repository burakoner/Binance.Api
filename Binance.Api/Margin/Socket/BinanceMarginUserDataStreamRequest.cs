namespace Binance.Api.Margin;

internal sealed class BinanceMarginUserDataStreamRequest : BinanceSocketQuery
{
    [JsonIgnore]
    public long? SubscriptionId { get; set; }

    [JsonIgnore]
    public long? ExpirationTime { get; set; }
}
