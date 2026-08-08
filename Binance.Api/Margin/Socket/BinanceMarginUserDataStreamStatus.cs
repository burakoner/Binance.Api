namespace Binance.Api.Margin;

internal sealed class BinanceMarginUserDataStreamStatus
{
    public int SubscriptionId { get; }
    public long ExpirationTime { get; }

    public BinanceMarginUserDataStreamStatus(int subscriptionId, long expirationTime)
    {
        SubscriptionId = subscriptionId;
        ExpirationTime = expirationTime;
    }
}
