namespace Binance.Api.Futures;

/// <summary>
/// Binance Coin Futures Market Data Web Socket Stream API
/// </summary>
public interface IBinanceFuturesSocketClientCoinStreamMarketData
{
    /// <summary>
    /// Subscribes to the aggregated trades update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Aggregate-Trade-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the aggregated trades update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Aggregate-Trade-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the Index price update stream for a single pair
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Index-Price-Stream" /></para>
    /// </summary>
    /// <param name="pair">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(string pair, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamIndexPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the Index price update stream for a list of pairs
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Index-Price-Stream" /></para>
    /// </summary>
    /// <param name="pairs">The pairs, for example `BTCUSD`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(IEnumerable<string> pairs, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamIndexPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the Mark price update stream for a single symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Mark-Price-Stream" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesCoinStreamMarkPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the Mark price update stream for a list of symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Mark-Price-of-All-Symbols-of-a-Pair" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesCoinStreamMarkPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    ///Subscribe to the Mark price update stream for all symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Mark-Price-of-All-Symbols-of-a-Pair" /></para>
    /// </summary>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(Action<WebSocketDataEvent<List<BinanceFuturesCoinStreamMarkPrice>>> onMessage, int? updateInterval = null, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbol and intervals
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="intervals">The intervals of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbols and intervals
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="intervals">The intervals of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the continuous contract candlestick update stream for the provided pair
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Continuous-Contract-Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="pair">The pair, for example `BTCUSD`</param>
    /// <param name="contractType">The contract type</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the continuous contract candlestick update stream for the provided pairs
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Continuous-Contract-Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="pairs">The pairs, for example `BTCUSD`</param>
    /// <param name="contractType">The contract type</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(IEnumerable<string> pairs, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the index candlestick update stream for the provided pair
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Index-Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="pair">The pair, for example `BTCUSD`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(string pair, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceStreamIndexKlineData>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the index candlestick update stream for the provided pairs
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Index-Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="pairs">The pairs, for example `BTCUSD`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(IEnumerable<string> pairs, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceStreamIndexKlineData>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the mark price candlestick update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Mark-Price-Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamIndexKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the mark price candlestick update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Mark-Price-Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamIndexKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to mini ticker updates stream for a specific symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Individual-Symbol-Mini-Ticker-Stream" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe to, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamCoinMiniTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to mini ticker updates stream for a list of symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Individual-Symbol-Mini-Ticker-Stream" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe to, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamCoinMiniTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to mini ticker updates stream for all symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/All-Market-Mini-Tickers-Stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamCoinMiniTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to ticker updates stream for a specific symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Individual-Symbol-Ticker-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe to, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamCoinTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to ticker updates stream for a specific symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Individual-Symbol-Ticker-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe to, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamCoinTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to ticker updates stream for all symbols
    /// <para><a href="https://binance-docs.github.io/apidocs/delivery/en/#all-market-tickers-streams" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamCoinTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the book ticker update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Individual-Symbol-Book-Ticker-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the book ticker update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Individual-Symbol-Book-Ticker-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to all book ticker update streams
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/All-Market-Tickers-Streams" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to specific symbol forced liquidations stream
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Liquidation-Order-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to list of symbol forced liquidations stream
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Liquidation-Order-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to all forced liquidations stream
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/All-Market-Liquidation-Order-Streams" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to contract/symbol updates
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Contract-Info-Stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToSymbolUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamSymbolUpdate>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the depth updates for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Partial-Book-Depth-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe on, for example `BTCUSD_PERP`</param>
    /// <param name="levels">The amount of entries to be returned in the update</param>
    /// <param name="updateInterval">Update interval in milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the depth updates for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Partial-Book-Depth-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe on, for example `BTCUSD_PERP`</param>
    /// <param name="levels">The amount of entries to be returned in the update of each symbol</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 100 or 500. Defaults to 250</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the order book updates for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Diff-Book-Depth-Streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 0 or 100, 500 or 1000, depending on endpoint</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the depth update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Diff-Book-Depth-Streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 0 or 100, 500 or 1000, depending on endpoint</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to individual trade update. NOTE: This endpoint stream isn't document and therefor might be changed or removed without prior notice
    /// </summary>
    /// <param name="symbol">Symbol to subscribe, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to individual trade update. NOTE: This endpoint stream isn't document and therefor might be changed or removed without prior notice
    /// </summary>
    /// <param name="symbols">Symbols to subscribe, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamTrade>> onMessage, CancellationToken ct = default);
}