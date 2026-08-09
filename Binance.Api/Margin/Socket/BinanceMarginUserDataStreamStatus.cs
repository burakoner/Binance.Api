namespace Binance.Api.Margin;

internal sealed class BinanceMarginUserDataStreamStatus
{
    public long SubscriptionId { get; }
    public long ExpirationTime { get; }

    public BinanceMarginUserDataStreamStatus(long subscriptionId, long expirationTime)
    {
        SubscriptionId = subscriptionId;
        ExpirationTime = expirationTime;
    }
}
