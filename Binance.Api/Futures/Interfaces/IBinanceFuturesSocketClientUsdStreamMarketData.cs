namespace Binance.Api.Futures;

/// <summary>
/// Binance USD Futures Market Data Web Socket Stream API
/// </summary>
public interface IBinanceFuturesSocketClientUsdStreamMarketData
{
    /// <summary>
    /// Subscribes to the aggregated trades update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Aggregate-Trade-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the aggregated trades update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Aggregate-Trade-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the Mark price update stream for a single symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Mark-Price-Stream" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPricesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the Mark price update stream for a list of symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Mark-Price-Stream" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPricesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the Mark price update stream for a all symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Mark-Price-Stream-for-All-market" /></para>
    /// </summary>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPricesAsync(int? updateInterval, Action<WebSocketDataEvent<IEnumerable<BinanceFuturesUsdtStreamMarkPrice>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="premiumIndex">Whether you want to subscribe to premium index k-lines</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage, bool premiumIndex = false, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbol and intervals
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="intervals">The intervals of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="premiumIndex">Whether you want to subscribe to premium index k-lines</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage, bool premiumIndex = false, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="premiumIndex">Whether you want to subscribe to premium index k-lines</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage, bool premiumIndex = false, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbols and intervals
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="intervals">The intervals of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="premiumIndex">Whether you want to subscribe to premium index k-lines</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage, bool premiumIndex = false, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the continuous contract candlestick update stream for the provided pair
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Continuous-Contract-Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="pair">The pair, for example `ETHUSDT`</param>
    /// <param name="contractType">The contract type</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlinesAsync(string pair, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the continuous contract candlestick update stream for the provided pairs
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Continuous-Contract-Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="pairs">The pairs, for example `ETHUSDT`</param>
    /// <param name="contractType">The contract type</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlinesAsync(IEnumerable<string> pairs, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to mini ticker updates stream for a specific symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Individual-Symbol-Mini-Ticker-Stream" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to mini ticker updates stream for a list of symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Individual-Symbol-Mini-Ticker-Stream" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to ticker updates stream for all symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/All-Market-Tickers-Streams" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(Action<WebSocketDataEvent<IEnumerable<BinanceFuturesStreamTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to ticker updates stream for a specific symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Individual-Symbol-Ticker-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to ticker updates stream for a specific symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Individual-Symbol-Ticker-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to mini ticker updates stream for all symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/All-Market-Mini-Tickers-Stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(Action<WebSocketDataEvent<IEnumerable<BinanceFuturesStreamMiniTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the book ticker update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Individual-Symbol-Book-Ticker-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the book ticker update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Individual-Symbol-Book-Ticker-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to all book ticker update streams
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/All-Book-Tickers-Stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to specific symbol forced liquidations stream
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Liquidation-Order-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationsAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to list of symbol forced liquidations stream
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Liquidation-Order-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationsAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to all forced liquidations stream
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/All-Market-Liquidation-Order-Streams" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationsAsync(Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the depth updates for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Partial-Book-Depth-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe on, for example `ETHUSDT`</param>
    /// <param name="levels">The amount of entries to be returned in the update, 5, 10 or 20</param>
    /// <param name="updateInterval">Update interval in milliseconds, 100, 250 or 500</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the depth updates for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Partial-Book-Depth-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe on, for example `ETHUSDT`</param>
    /// <param name="levels">The amount of entries to be returned in the update of each symbol</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 100 or 500. Defaults to 250</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the order book updates for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Diff-Book-Depth-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 0 or 100, 500 or 1000, depending on endpoint</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the depth update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Diff-Book-Depth-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 0 or 100, 500 or 1000, depending on endpoint</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to composite index updates stream for a symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Composite-Index-Symbol-Information-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToCompositeIndexesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamCompositeIndex>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to contract/symbol updates
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Contract-Info-Stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToSymbolsAsync(Action<WebSocketDataEvent<BinanceFuturesStreamSymbolUpdate>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to asset index updates for a single
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Multi-Assets-Mode-Asset-Index" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamAssetIndexUpdate>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to asset index updates stream
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/websocket-market-streams/Multi-Assets-Mode-Asset-Index" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexesAsync(Action<WebSocketDataEvent<IEnumerable<BinanceFuturesStreamAssetIndexUpdate>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to individual trade update. NOTE: This endpoint stream isn't document and therefor might be changed or removed without prior notice
    /// </summary>
    /// <param name="symbol">Symbol to subscribe, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="filterOutNonTradeUpdates">Filter out any update which isn't a trade. Occasionally different updates (like INSURANCE_FUND updates) will occur on this stream. By default these are ignored</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamTrade>> onMessage, bool filterOutNonTradeUpdates = true, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to individual trade update. NOTE: This endpoint stream isn't document and therefor might be changed or removed without prior notice
    /// </summary>
    /// <param name="symbols">Symbols to subscribe, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="filterOutNonTradeUpdates">Filter out any update which isn't a trade. Occasionally different updates (like INSURANCE_FUND updates) will occur on this stream. By default these are ignored</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamTrade>> onMessage, bool filterOutNonTradeUpdates = true, CancellationToken ct = default);
}