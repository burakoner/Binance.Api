using Binance.Api.Spot;

namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientCoin
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        string symbol,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync([symbol], [interval], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        string symbol,
        IEnumerable<BinanceKlineInterval> intervals,
        Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync([symbol], intervals, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        IEnumerable<string> symbols,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync(symbols, [interval], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        IEnumerable<string> symbols,
        IEnumerable<BinanceKlineInterval> intervals,
        Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamCoinKlineData>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Data));
        });

        var topics = symbols.SelectMany(a => intervals.Select(i => a.ToLower(BinanceConstants.CI) + "@kline" + "_" + MapConverter.GetString(i))).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(
        string pair,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamIndexPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToIndexPriceUpdatesAsync([pair], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(
        IEnumerable<string> pairs,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamIndexPrice>> onMessage,
        CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamIndexPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });

        var topics = pairs.Select(a => a.ToLower(BinanceConstants.CI) + "@indexPrice" + (updateInterval == 1000 ? "@1s" : "")).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(
        string symbol,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesCoinStreamMarkPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToMarkPriceUpdatesAsync([symbol], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(
        IEnumerable<string> symbols,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesCoinStreamMarkPrice>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesCoinStreamMarkPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });

        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@markPrice" + (updateInterval == 1000 ? "@1s" : "")).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(
        string pair,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceStreamKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToContinuousContractKlineUpdatesAsync([pair], contractType, interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(
        IEnumerable<string> pairs,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceStreamKline>> onMessage,
        CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamKlineData>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Data));
        });
        var topics = pairs.Select(a =>
            a.ToLower(BinanceConstants.CI) + "_" +
            MapConverter.GetString(contractType).ToLower() + "@continuousKline" + "_" +
            MapConverter.GetString(interval)).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(
        string pair,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceStreamIndexKlineData>> onMessage,
        CancellationToken ct = default)
        => SubscribeToIndexKlineUpdatesAsync([pair], interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(
        IEnumerable<string> pairs,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceStreamIndexKlineData>> onMessage,
        CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamIndexKlineData>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        var topics = pairs.Select(a => a.ToLower(BinanceConstants.CI) + "@indexPriceKline" + "_" + MapConverter.GetString(interval)).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(
        string symbol,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamIndexKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToMarkPriceKlineUpdatesAsync([symbol], interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(
        IEnumerable<string> symbols,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamIndexKline>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamIndexKlineData>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Data));
        });
        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@markPriceKline" + "_" + MapConverter.GetString(interval)).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceStreamCoinMiniTick>> onMessage,
        CancellationToken ct = default)
        => SubscribeToMiniTickerUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceStreamCoinMiniTick>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamCoinMiniTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@miniTicker").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(
        Action<WebSocketDataEvent<IEnumerable<BinanceStreamCoinMiniTick>>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceStreamCoinMiniTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!miniTicker@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<IBinance24HPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToTickerUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<IBinance24HPrice>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamCoinTick>>>(data =>
        {
            onMessage(data.As<IBinance24HPrice>(data.Data.Data));
        });
        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@ticker").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllTickerUpdatesAsync(
        Action<WebSocketDataEvent<IEnumerable<IBinance24HPrice>>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceStreamCoinTick>>>>(data =>
        {
            onMessage(data.As<IEnumerable<IBinance24HPrice>>(data.Data.Data));
        });
        return SubscribeAsync(["!ticker@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceSpotStreamAggregatedTrade>> onMessage,
        CancellationToken ct = default)
        => SubscribeToAggregatedTradeUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceSpotStreamAggregatedTrade>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceSpotStreamAggregatedTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@aggTrade").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage, CancellationToken ct = default)
        => SubscribeToTradeUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@trade").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(
        Action<WebSocketDataEvent<IEnumerable<BinanceFuturesCoinStreamMarkPrice>>> onMessage,
        int? updateInterval = null,
        CancellationToken ct = default)
    {
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceFuturesCoinStreamMarkPrice>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!markPrice@arr" + (updateInterval == 1000 ? "@1s" : "")], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToBookTickerUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@bookTicker").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(
        Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!bookTicker"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage,
        CancellationToken ct = default)
        => SubscribeToLiquidationUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidationData>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Data));
        });
        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@forceOrder").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(
        Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidationData>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Data));
        });
        return SubscribeAsync(["!forceOrder@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(
        string symbol,
        int levels,
        int? updateInterval,
        Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage,
        CancellationToken ct = default)
        => SubscribeToPartialOrderBookUpdatesAsync([symbol], levels, updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(
        IEnumerable<string> symbols,
        int levels,
        int? updateInterval,
        Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream?.Split('@')[0] ?? "";
            onMessage(data.As<IBinanceFuturesEventOrderBook>(data.Data.Data));
        });

        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
        string symbol,
        int? updateInterval,
        Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage,
        CancellationToken ct = default)
        => SubscribeToOrderBookUpdatesAsync([symbol], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
        IEnumerable<string> symbols,
        int? updateInterval,
        Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
        {
            onMessage(data.As<IBinanceFuturesEventOrderBook>(data.Data.Data));
        });
        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToSymbolUpdatesAsync(
        Action<WebSocketDataEvent<BinanceFuturesStreamSymbolUpdate>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamSymbolUpdate>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!contractInfo"], false, handler, ct);
    }
}