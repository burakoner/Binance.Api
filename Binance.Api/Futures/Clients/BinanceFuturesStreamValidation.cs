namespace Binance.Api.Futures;

internal static class BinanceFuturesStreamValidation
{
    private static readonly BinanceFuturesContractType[] UsdContinuousKlineContractTypes =
    [
        BinanceFuturesContractType.Perpetual,
        BinanceFuturesContractType.CurrentQuarter,
        BinanceFuturesContractType.NextQuarter,
        BinanceFuturesContractType.TradFiPerpetual
    ];

    private static readonly BinanceFuturesContractType[] CoinContinuousKlineContractTypes =
    [
        BinanceFuturesContractType.Perpetual,
        BinanceFuturesContractType.CurrentQuarter,
        BinanceFuturesContractType.NextQuarter
    ];

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

    public static string[] UsdContinuousKlineStreamTopics(
        IEnumerable<string> pairs,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval)
        => ContinuousKlineStreamTopics(
            pairs,
            contractType,
            interval,
            UsdContinuousKlineContractTypes,
            supportsOneSecond: true);

    public static string[] CoinContinuousKlineStreamTopics(
        IEnumerable<string> pairs,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval)
        => ContinuousKlineStreamTopics(
            pairs,
            contractType,
            interval,
            CoinContinuousKlineContractTypes,
            supportsOneSecond: false);

    public static string[] CoinMarkPriceKlineStreamTopics(
        IEnumerable<string> symbols,
        BinanceKlineInterval interval)
    {
        if (symbols == null)
            throw new ArgumentNullException(nameof(symbols));

        var symbolList = symbols.ToArray();
        if (symbolList.Length == 0)
            throw new ArgumentException("At least one symbol is required.", nameof(symbols));
        if (symbolList.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Symbols cannot be null or blank.", nameof(symbols));

        if (!Enum.IsDefined(typeof(BinanceKlineInterval), interval)
            || interval == BinanceKlineInterval.OneSecond
            || MapConverter.GetString(interval) == null)
            throw new ArgumentOutOfRangeException(nameof(interval), "The COIN-M mark price kline stream does not support the provided interval.");

        if (symbolList.Length > 1024)
            throw new ArgumentException("A single connection can listen to at most 1024 streams.", nameof(symbols));

        var intervalText = MapConverter.GetString(interval);
        return symbolList.Select(symbol =>
            symbol.ToLower(BinanceConstants.CI) + "@markPriceKline_" + intervalText).ToArray();
    }

    public static string[] CoinIndexPriceKlineStreamTopics(
        IEnumerable<string> pairs,
        BinanceKlineInterval interval)
    {
        if (pairs == null)
            throw new ArgumentNullException(nameof(pairs));

        var pairList = pairs.ToArray();
        if (pairList.Length == 0)
            throw new ArgumentException("At least one pair is required.", nameof(pairs));
        if (pairList.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Pairs cannot be null or blank.", nameof(pairs));

        if (!Enum.IsDefined(typeof(BinanceKlineInterval), interval)
            || interval == BinanceKlineInterval.OneSecond
            || MapConverter.GetString(interval) == null)
            throw new ArgumentOutOfRangeException(nameof(interval), "The COIN-M index price kline stream does not support the provided interval.");

        if (pairList.Length > 1024)
            throw new ArgumentException("A single connection can listen to at most 1024 streams.", nameof(pairs));

        var intervalText = MapConverter.GetString(interval);
        return pairList.Select(pair =>
            pair.ToLower(BinanceConstants.CI) + "@indexPriceKline_" + intervalText).ToArray();
    }

    private static string[] ContinuousKlineStreamTopics(
        IEnumerable<string> pairs,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval,
        IReadOnlyCollection<BinanceFuturesContractType> supportedContractTypes,
        bool supportsOneSecond)
    {
        if (pairs == null)
            throw new ArgumentNullException(nameof(pairs));

        var pairList = pairs.ToArray();
        if (pairList.Length == 0)
            throw new ArgumentException("At least one pair is required.", nameof(pairs));
        if (pairList.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Pairs cannot be null or blank.", nameof(pairs));

        if (!Enum.IsDefined(typeof(BinanceFuturesContractType), contractType)
            || !supportedContractTypes.Contains(contractType))
            throw new ArgumentOutOfRangeException(nameof(contractType), "The continuous kline stream does not support the provided contract type.");

        if (!Enum.IsDefined(typeof(BinanceKlineInterval), interval)
            || (!supportsOneSecond && interval == BinanceKlineInterval.OneSecond)
            || MapConverter.GetString(interval) == null)
            throw new ArgumentOutOfRangeException(nameof(interval), "The continuous kline stream does not support the provided interval.");

        if (pairList.Length > 1024)
            throw new ArgumentException("A single connection can listen to at most 1024 streams.", nameof(pairs));

        var contractTypeText = MapConverter.GetString(contractType)!.ToLower(BinanceConstants.CI);
        var intervalText = MapConverter.GetString(interval);
        return pairList.Select(pair =>
            pair.ToLower(BinanceConstants.CI) + "_" + contractTypeText + "@continuousKline_" + intervalText).ToArray();
    }
}
