namespace Binance.Api.Futures;

internal static class BinanceFuturesStreamValidation
{
    public static string[] KlineStreamTopics(
        IEnumerable<string> symbols,
        IEnumerable<BinanceKlineInterval> intervals)
    {
        if (symbols == null)
            throw new ArgumentNullException(nameof(symbols));
        if (intervals == null)
            throw new ArgumentNullException(nameof(intervals));

        var symbolList = symbols.ToArray();
        if (symbolList.Length == 0)
            throw new ArgumentException("At least one symbol is required.", nameof(symbols));
        if (symbolList.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Symbols cannot be null or blank.", nameof(symbols));

        var intervalList = intervals.ToArray();
        if (intervalList.Length == 0)
            throw new ArgumentException("At least one kline interval is required.", nameof(intervals));
        if (intervalList.Any(interval =>
                !Enum.IsDefined(typeof(BinanceKlineInterval), interval)
                || interval == BinanceKlineInterval.OneSecond
                || MapConverter.GetString(interval) == null))
            throw new ArgumentOutOfRangeException(nameof(intervals), "Futures kline streams do not support the provided interval.");

        var topics = symbolList.SelectMany(symbol => intervalList.Select(interval =>
            symbol.ToLower(BinanceConstants.CI) + "@kline_" + MapConverter.GetString(interval))).ToArray();
        if (topics.Length > 1024)
            throw new ArgumentException("A single connection can listen to at most 1024 streams.", nameof(symbols));

        return topics;
    }
}
