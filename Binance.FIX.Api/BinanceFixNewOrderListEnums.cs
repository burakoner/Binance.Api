namespace Binance.FIX.Api;

/// <summary>Current Binance Spot FIX order-list shape.</summary>
public enum BinanceFixOrderListType
{
    /// <summary>One-cancels-the-other list.</summary>
    OneCancelsTheOther = 1,

    /// <summary>One-triggers-the-other list.</summary>
    OneTriggersTheOther = 2,

    /// <summary>One-triggers-the-other, one-cancels-the-other list.</summary>
    OneTriggersTheOtherOneCancelsTheOther = 3,

    /// <summary>One-pays-the-other list.</summary>
    OnePaysTheOther = 4,

    /// <summary>One-pays-the-other, one-cancels-the-other list.</summary>
    OnePaysTheOtherOneCancelsTheOther = 5
}
