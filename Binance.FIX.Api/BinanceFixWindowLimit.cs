using System;

namespace Binance.FIX.Api;

/// <summary>
/// An immutable Binance FIX rolling-window count limit.
/// </summary>
public sealed class BinanceFixWindowLimit
{
    internal BinanceFixWindowLimit(int maximumCount, TimeSpan window)
    {
        if (maximumCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumCount));
        }

        if (window <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(window));
        }

        MaximumCount = maximumCount;
        Window = window;
    }

    /// <summary>
    /// Gets the maximum permitted count in the rolling window.
    /// </summary>
    public int MaximumCount { get; }

    /// <summary>
    /// Gets the rolling-window duration.
    /// </summary>
    public TimeSpan Window { get; }
}
