namespace Binance.Api.Futures;

/// <summary>
/// Binance USD Futures Market Data Web Socket Stream API
/// </summary>
public interface IBinanceFuturesSocketClientUsdStreamMarketData
{
    /// <summary>
    /// Subscribes to the aggregated trades update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#aggregate-trade-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesUsdtStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the aggregated trades update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#aggregate-trade-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesUsdtStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the Mark price update stream for a single symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#mark-price-stream" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="updateInterval">Update interval in milliseconds. Use 1000 for the explicit 1-second stream; null or 3000 uses the default 3-second stream</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPricesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the Mark price update stream for a list of symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#mark-price-stream" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="updateInterval">Update interval in milliseconds. Use 1000 for the explicit 1-second stream; null or 3000 uses the default 3-second stream</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPricesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the Mark price update stream for all symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#mark-price-stream-for-all-market" /></para>
    /// </summary>
    /// <param name="updateInterval">Update interval in milliseconds. Use 1000 for the explicit 1-second stream; null or 3000 uses the default 3-second stream</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPricesAsync(int? updateInterval, Action<WebSocketDataEvent<List<BinanceFuturesStreamAllMarketMarkPrice>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the candlestick update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#klinecandlestick-streams" /></para>
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
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#klinecandlestick-streams" /></para>
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
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#klinecandlestick-streams" /></para>
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
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#klinecandlestick-streams" /></para>
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
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#continuous-contract-klinecandlestick-streams" /></para>
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
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#continuous-contract-klinecandlestick-streams" /></para>
    /// </summary>
    /// <param name="pairs">The pairs, for example `ETHUSDT`</param>
    /// <param name="contractType">The contract type</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlinesAsync(IEnumerable<string> pairs, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to 24-hour mini ticker updates for a USDⓈ-M symbol. Use the symbol-type-aware volume properties on the callback model.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#individual-symbol-mini-ticker-stream" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to 24-hour mini ticker updates for USDⓈ-M symbols. Use the symbol-type-aware volume properties on the callback model.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#individual-symbol-mini-ticker-stream" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the merged USDⓈ-M and COIN-M 24-hour ticker stream. Use <c>SymbolType</c> and the product-safe volume properties.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#all-market-tickers-streams" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to 24-hour ticker updates for a USDⓈ-M symbol. Use the symbol-type-aware volume properties on the callback model.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#individual-symbol-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to 24-hour ticker updates for USDⓈ-M symbols. Use the symbol-type-aware volume properties on the callback model.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#individual-symbol-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe to, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the merged USDⓈ-M and COIN-M 24-hour mini ticker stream. Use <c>SymbolType</c> and the product-safe volume properties.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#all-market-mini-tickers-stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamMiniTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the book ticker update stream for the provided symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/public#individual-symbol-book-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the book ticker update stream for the provided symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/public#individual-symbol-book-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to all book ticker update streams
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/public#all-book-tickers-stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to liquidation-order snapshots for a specific symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#liquidation-order-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationsAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to liquidation-order snapshots for the provided symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#liquidation-order-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationsAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to merged USDⓈ-M and COIN-M all-market liquidation-order snapshots
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#all-market-liquidation-order-streams" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationsAsync(Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to standard partial order-book depth updates for the provided symbol. RPI orders are excluded
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/public#partial-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe on, for example `ETHUSDT`</param>
    /// <param name="levels">The amount of entries to be returned in the update, 5, 10 or 20</param>
    /// <param name="updateInterval">Explicit update interval in milliseconds, either 100 or 500. Null uses the default 250 milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to standard partial order-book depth updates for the provided symbols. RPI orders are excluded
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/public#partial-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe on, for example `ETHUSDT`</param>
    /// <param name="levels">The amount of entries to be returned in each update, 5, 10 or 20</param>
    /// <param name="updateInterval">Explicit update interval in milliseconds, either 100 or 500. Null uses the default 250 milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to standard diff order-book depth updates for the provided symbol. RPI orders are excluded
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/public#diff-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="updateInterval">Explicit update interval in milliseconds, either 100 or 500. Null uses the default 250 milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to standard diff order-book depth updates for the provided symbols. RPI orders are excluded
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/public#diff-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="updateInterval">Explicit update interval in milliseconds, either 100 or 500. Null uses the default 250 milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to diff order-book depth updates including aggregated RPI orders for the provided symbol. Updates are pushed every 500 milliseconds
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/public#rpi-diff-book-depth-streams" /></para>
    /// <para>A zero update quantity can mean all quotations at that price were filled or canceled, or that crossed RPI quantity is hidden</para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRpiOrderBooksAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to diff order-book depth updates including aggregated RPI orders for the provided symbols. Updates are pushed every 500 milliseconds
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/public#rpi-diff-book-depth-streams" /></para>
    /// <para>A zero update quantity can mean all quotations at that price were filled or canceled, or that crossed RPI quantity is hidden</para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRpiOrderBooksAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to composite index updates stream for a symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#composite-index-symbol-information-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToCompositeIndexesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamCompositeIndex>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to contract/symbol updates
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#contract-info-stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToSymbolsAsync(Action<WebSocketDataEvent<BinanceFuturesStreamSymbolUpdate>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to asset index updates for a single
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#multi-assets-mode-asset-index" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamAssetIndexUpdate>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to asset index updates stream
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#multi-assets-mode-asset-index" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexesAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamAssetIndexUpdate>>> onMessage, CancellationToken ct = default);

}
