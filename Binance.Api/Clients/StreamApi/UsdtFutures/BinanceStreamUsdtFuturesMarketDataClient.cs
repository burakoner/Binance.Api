﻿using Binance.Api.Models.StreamApi.Futures;

namespace Binance.Api.Clients.StreamApi.UsdtFutures;

public class BinanceStreamUsdtFuturesMarketDataClient
{
    private const string klineStreamEndpoint = "@kline";
    private const string continuousContractKlineStreamEndpoint = "@continuousKline";
    private const string markPriceStreamEndpoint = "@markPrice";
    private const string allMarkPriceStreamEndpoint = "!markPrice@arr";
    private const string symbolMiniTickerStreamEndpoint = "@miniTicker";
    private const string allMiniTickerStreamEndpoint = "!miniTicker@arr";
    private const string symbolTickerStreamEndpoint = "@ticker";
    private const string allTickerStreamEndpoint = "!ticker@arr";
    private const string compositeIndexEndpoint = "@compositeIndex";

    private const string aggregatedTradesStreamEndpoint = "@aggTrade";
    private const string bookTickerStreamEndpoint = "@bookTicker";
    private const string allBookTickerStreamEndpoint = "!bookTicker";
    private const string liquidationStreamEndpoint = "@forceOrder";
    private const string allLiquidationStreamEndpoint = "!forceOrder@arr";
    private const string partialBookDepthStreamEndpoint = "@depth";
    private const string depthStreamEndpoint = "@depth";

    // Internal References
    internal BinanceStreamUsdtFuturesClient MainClient { get; }
    internal Log Log { get => MainClient.Log; }
    internal string BaseAddress{ get => Options.BaseAddress; }
    internal BinanceStreamClientOptions Options { get => MainClient.RootClient.Options; }
    internal CallResult<T> Deserialize<T>(string data, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(data, serializer, requestId);
    internal CallResult<T> Deserialize<T>(JToken obj, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(obj, serializer, requestId);
    internal Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<StreamDataEvent<T>> onData, CancellationToken ct)
    => MainClient.SubscribeAsync<T>(url, topics, onData, ct);

    internal BinanceStreamUsdtFuturesMarketDataClient(BinanceStreamUsdtFuturesClient main)
    {
        MainClient = main;
    }

    #region Mark Price Stream
    public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, int? updateInterval, Action<StreamDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default) => await SubscribeToMarkPriceUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<StreamDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceFuturesUsdtStreamMarkPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + markPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "")).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Mark Price Stream for All market
    public async Task<CallResult<UpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(int? updateInterval, Action<StreamDataEvent<IEnumerable<BinanceFuturesStreamMarkPrice>>> onMessage, CancellationToken ct = default)
    {
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<IEnumerable<BinanceFuturesStreamMarkPrice>>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync(BaseAddress, new[] { allMarkPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "") }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Kline/Candlestick Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<StreamDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToKlineUpdatesAsync(new[] { symbol }, interval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<KlineInterval> intervals, Action<StreamDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToKlineUpdatesAsync(new[] { symbol }, intervals, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<StreamDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) =>
        await SubscribeToKlineUpdatesAsync(symbols, new[] { interval }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<KlineInterval> intervals, Action<StreamDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamKlineData>>>(data => onMessage(data.As<IBinanceStreamKlineData>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.SelectMany(a => intervals.Select(i => a.ToLower(CultureInfo.InvariantCulture) + klineStreamEndpoint + "_" + JsonConvert.SerializeObject(i, new KlineIntervalConverter(false)))).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Continuous contract kline/Candlestick Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair, ContractType contractType, KlineInterval interval, Action<StreamDataEvent<BinanceStreamContinuousKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToContinuousContractKlineUpdatesAsync(new[] { pair }, contractType, interval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(IEnumerable<string> pairs, ContractType contractType, KlineInterval interval, Action<StreamDataEvent<BinanceStreamContinuousKlineData>> onMessage, CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamContinuousKlineData>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        pairs = pairs.Select(a => a.ToLower(CultureInfo.InvariantCulture) +
                                  "_" +
                                  JsonConvert.SerializeObject(contractType, new ContractTypeConverter(false)).ToLower() +
                                  continuousContractKlineStreamEndpoint +
                                  "_" +
                                  JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))).ToArray();
        return await SubscribeAsync(BaseAddress, pairs, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Mini Ticker Stream
    public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<StreamDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default) => await SubscribeToMiniTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<StreamDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamMiniTick>>>(data => onMessage(data.As<IBinanceMiniTick>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolMiniTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Mini Tickers Stream
    public async Task<CallResult<UpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<StreamDataEvent<IEnumerable<IBinanceMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamMiniTick>>>>(data => onMessage(data.As<IEnumerable<IBinanceMiniTick>>(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync(BaseAddress, new[] { allMiniTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Ticker Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<StreamDataEvent<IBinance24HPrice>> onMessage, CancellationToken ct = default) => await SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<StreamDataEvent<IBinance24HPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamTick>>>(data => onMessage(data.As<IBinance24HPrice>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Composite index Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToCompositeIndexUpdatesAsync(string symbol, Action<StreamDataEvent<BinanceFuturesStreamCompositeIndex>> onMessage, CancellationToken ct = default)
    {
        var action = new Action<StreamDataEvent<BinanceCombinedStream<BinanceFuturesStreamCompositeIndex>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });
        return await SubscribeAsync(BaseAddress, new[] { symbol + compositeIndexEndpoint }, action, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Tickers Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<StreamDataEvent<IEnumerable<IBinance24HPrice>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamTick>>>>(data => onMessage(data.As<IEnumerable<IBinance24HPrice>>(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync(BaseAddress, new[] { allTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Aggregate Trade Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<StreamDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default) => await SubscribeToAggregatedTradeUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<StreamDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamAggregatedTrade>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + aggregatedTradesStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Book Ticker Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<StreamDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default) => await SubscribeToBookTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<StreamDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceFuturesStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + bookTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Book Tickers Stream
    public async Task<CallResult<UpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(Action<StreamDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        return await SubscribeAsync(BaseAddress, new[] { allBookTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Liquidation Order Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(string symbol, Action<StreamDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default) => await SubscribeToLiquidationUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(IEnumerable<string> symbols, Action<StreamDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceFuturesStreamLiquidationData>>>(data => onMessage(data.As(data.Data.Data.Data, data.Data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + liquidationStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Liquidation Order Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(Action<StreamDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceFuturesStreamLiquidationData>>>(data => onMessage(data.As(data.Data.Data.Data, data.Data.Data.Data.Symbol)));
        return await SubscribeAsync(BaseAddress, new[] { allLiquidationStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Partial Book Depth Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int levels, int? updateInterval, Action<StreamDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default) => await SubscribeToPartialOrderBookUpdatesAsync(new[] { symbol }, levels, updateInterval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<StreamDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream.Split('@')[0];
            onMessage(data.As<IBinanceFuturesEventOrderBook>(data.Data.Data, data.Data.Data.Symbol));
        });

        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + partialBookDepthStreamEndpoint + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Diff. Book Depth Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int? updateInterval, Action<StreamDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default) => await SubscribeToOrderBookUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<StreamDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data => onMessage(data.As<IBinanceFuturesEventOrderBook>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + depthStreamEndpoint + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion
}