namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot Web Socket API Client Market Data Stream Methods
/// </summary>
public interface IBinanceSpotSocketClientStreamMarketData
{
    /// <summary>
    /// Subscribes to the aggregated trades update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#aggregate-trade-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the aggregated trades update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#aggregate-trade-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the trades update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#trade-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the trades update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#trade-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#klinecandlestick-streams-for-utc" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbol and intervals
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#klinecandlestick-streams-for-utc" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="intervals">The intervals of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#klinecandlestick-streams-for-utc" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbols and intervals
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#klinecandlestick-streams-for-utc" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="intervals">The intervals of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default);

    // TODO: Kline/Candlestick Streams with timezone offset

    /// <summary>
    /// Subscribes to mini ticker updates stream for a specific symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#individual-symbol-mini-ticker-stream" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamMiniTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to mini ticker updates stream for a list of symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#individual-symbol-mini-ticker-stream" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamMiniTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to mini ticker updates stream for all symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#all-market-mini-tickers-stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(Action<WebSocketDataEvent<IEnumerable<BinanceStreamMiniTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to ticker updates stream for a specific symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#individual-symbol-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<WebSocketDataEvent<IBinanceTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to ticker updates stream for a specific symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#individual-symbol-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinanceTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to ticker updates stream for all symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#all-market-tickers-stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(Action<WebSocketDataEvent<IEnumerable<BinanceStreamTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to rolling window ticker updates stream for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#individual-symbol-rolling-window-statistics-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe, for example `ETHUSDT`</param>
    /// <param name="windowSize">Window size, either 1 hour or 4 hours</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRollingWindowTickersAsync(string symbol, TimeSpan windowSize, Action<WebSocketDataEvent<BinanceStreamRollingWindowTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to rolling window ticker updates stream for all symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#all-market-rolling-window-statistics-streams" /></para>
    /// </summary>
    /// <param name="windowSize">Window size, either 1 hour or 4 hours</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRollingWindowTickersAsync(TimeSpan windowSize, Action<WebSocketDataEvent<IEnumerable<BinanceStreamRollingWindowTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the book ticker update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#individual-symbol-book-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the book ticker update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#individual-symbol-book-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the depth updates for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#partial-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe on, for example `ETHUSDT`</param>
    /// <param name="levels">The amount of entries to be returned in the update, 5, 10 or 20</param>
    /// <param name="updateInterval">Update interval in milliseconds, 1000ms or 100ms</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotOrderBook>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the depth updates for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#partial-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe on, for example `ETHUSDT`</param>
    /// <param name="levels">The amount of entries to be returned in the update of each symbol, 5, 10 or 20</param>
    /// <param name="updateInterval">Update interval in milliseconds, 1000ms or 100ms</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotOrderBook>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the order book updates for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#diff-depth-stream" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="updateInterval">Update interval in milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<IBinanceEventOrderBook>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the depth update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-streams#diff-depth-stream" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="updateInterval">Update interval in milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<IBinanceEventOrderBook>> onMessage, CancellationToken ct = default);
}