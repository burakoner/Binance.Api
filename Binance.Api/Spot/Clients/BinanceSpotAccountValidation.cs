namespace Binance.Api.Spot;

internal static class BinanceSpotAccountValidation
{
    private static readonly TimeSpan MaximumHistoryRange = TimeSpan.FromHours(24);

    public static string? ReceiveWindow(decimal? receiveWindow)
    {
        if (!receiveWindow.HasValue)
            return null;
        if (receiveWindow.Value < 0 || receiveWindow.Value > 60_000m)
            throw new ArgumentOutOfRangeException(nameof(receiveWindow), "Receive window must be between 0 and 60000 milliseconds.");
        if (receiveWindow.Value != decimal.Round(receiveWindow.Value, 3))
            throw new ArgumentException("Receive window supports at most three decimal places.", nameof(receiveWindow));

        return receiveWindow.Value.ToString("0.###", CultureInfo.InvariantCulture);
    }

    public static void HistoryRange(DateTime? startTime, DateTime? endTime)
    {
        if (startTime > endTime)
            throw new ArgumentException("startTime cannot be later than endTime.", nameof(startTime));
        if (endTime - startTime > MaximumHistoryRange)
            throw new ArgumentException("The time between startTime and endTime cannot exceed 24 hours.", nameof(endTime));
    }

    public static void UserTrades(long? orderId, DateTime? startTime, DateTime? endTime, long? fromId)
    {
        HistoryRange(startTime, endTime);
        if (fromId.HasValue && (startTime.HasValue || endTime.HasValue))
            throw new ArgumentException("fromId cannot be combined with startTime or endTime.", nameof(fromId));
        if (orderId.HasValue && (startTime.HasValue || endTime.HasValue))
            throw new ArgumentException("orderId cannot be combined with startTime or endTime.", nameof(orderId));
    }

    public static void OrderListHistory(long? fromId, DateTime? startTime, DateTime? endTime)
    {
        HistoryRange(startTime, endTime);
        if (fromId.HasValue && (startTime.HasValue || endTime.HasValue))
            throw new ArgumentException("fromId cannot be combined with startTime or endTime.", nameof(fromId));
    }

    public static void Allocations(long? orderId, long? fromAllocationId, DateTime? startTime, DateTime? endTime)
    {
        HistoryRange(startTime, endTime);
        if ((orderId.HasValue || fromAllocationId.HasValue) && (startTime.HasValue || endTime.HasValue))
            throw new ArgumentException("orderId and fromAllocationId cannot be combined with startTime or endTime.");
    }

    public static void PreventedMatches(long? orderId, long? preventedMatchId, long? fromPreventedMatchId, int? limit)
    {
        var valid = preventedMatchId.HasValue && !orderId.HasValue && !fromPreventedMatchId.HasValue && !limit.HasValue
            || orderId.HasValue && !preventedMatchId.HasValue && (!limit.HasValue || fromPreventedMatchId.HasValue);
        if (!valid)
            throw new ArgumentException("Use preventedMatchId alone, orderId alone, or orderId with fromPreventedMatchId and optional limit.");
    }
}
