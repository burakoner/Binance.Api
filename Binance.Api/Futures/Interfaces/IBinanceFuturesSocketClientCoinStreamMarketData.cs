namespace Binance.Api.Futures;

/// <summary>
/// Binance Coin Futures Market Data Web Socket Stream API
/// </summary>
public interface IBinanceFuturesSocketClientCoinStreamMarketData
{
    /// <summary>
    /// Subscribes to the 100-millisecond aggregate-trade stream for the provided symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#aggregate-trade-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesCoinStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the 100-millisecond aggregate-trade stream for the provided symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#aggregate-trade-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesCoinStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

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
    /// Subscribes to the mark-price update stream for a single symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#mark-price-stream" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesCoinStreamMarkPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the mark-price update stream for a list of symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#mark-price-stream" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesCoinStreamMarkPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the all-market mark-price update stream
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-streams/market#mark-price-stream-for-all-market" /></para>
    /// </summary>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamAllMarketMarkPrice>>> onMessage, int? updateInterval = null, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the COIN-M candlestick update stream for the provided symbol. Volumes use contract and base-asset units.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#klinecandlestick-streams" /></para>
    /// <para>The payload has no product discriminator; use this COIN-M callback only for COIN-M symbols.</para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the COIN-M candlestick update stream for the provided symbol and intervals. Volumes use contract and base-asset units.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#klinecandlestick-streams" /></para>
    /// <para>The payload has no product discriminator; use this COIN-M callback only for COIN-M symbols.</para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="intervals">The intervals of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the COIN-M candlestick update stream for the provided symbols. Volumes use contract and base-asset units.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#klinecandlestick-streams" /></para>
    /// <para>The payload has no product discriminator; use this COIN-M callback only for COIN-M symbols.</para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the COIN-M candlestick update stream for the provided symbols and intervals. Volumes use contract and base-asset units.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#klinecandlestick-streams" /></para>
    /// <para>The payload has no product discriminator; use this COIN-M callback only for COIN-M symbols.</para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="intervals">The intervals of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the COIN-M continuous contract candlestick stream for the provided pair. The callback retains pair and contract type; volumes use contract and base-asset units.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#continuous-contract-klinecandlestick-streams" /></para>
    /// <para>The payload has no product discriminator; use this COIN-M callback only for COIN-M pairs.</para>
    /// </summary>
    /// <param name="pair">The pair, for example `BTCUSD`</param>
    /// <param name="contractType">The contract type</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamCoinContinuousKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the COIN-M continuous contract candlestick stream for the provided pairs. The callback retains pair and contract type; volumes use contract and base-asset units.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#continuous-contract-klinecandlestick-streams" /></para>
    /// <para>The payload has no product discriminator; use this COIN-M callback only for COIN-M pairs.</para>
    /// </summary>
    /// <param name="pairs">The pairs, for example `BTCUSD`</param>
    /// <param name="contractType">The contract type</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(IEnumerable<string> pairs, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamCoinContinuousKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the index candlestick update stream for the provided pair
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Index-Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="pair">The pair, for example `BTCUSD`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(string pair, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamIndexKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to mark-price updates for all symbols of a pair
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#mark-price-of-all-symbols-of-a-pair" /></para>
    /// </summary>
    /// <param name="pair">The pair, for example `BTCUSD`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMarkPriceUpdatesOfAllSymbolsOfPairAsync(string pair, int? updateInterval, Action<WebSocketDataEvent<List<BinanceFuturesCoinStreamMarkPrice>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the index candlestick update stream for the provided pairs
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/websocket-market-streams/Index-Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="pairs">The pairs, for example `BTCUSD`</param>
    /// <param name="interval">The interval of the candlesticks</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(IEnumerable<string> pairs, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceFuturesStreamIndexKline>> onMessage, CancellationToken ct = default);

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
    /// Subscribes to 24-hour mini ticker updates for a COIN-M symbol. Use the symbol-type-aware volume properties on the callback model.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#individual-symbol-mini-ticker-stream" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe to, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to 24-hour mini ticker updates for COIN-M symbols. Use the symbol-type-aware volume properties on the callback model.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#individual-symbol-mini-ticker-stream" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe to, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the merged USDⓈ-M and COIN-M 24-hour mini ticker stream. Use <c>SymbolType</c> and the product-safe volume properties.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#all-market-mini-tickers-stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamMiniTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to 24-hour ticker updates for a COIN-M symbol. Use the symbol-type-aware volume properties on the callback model.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#individual-symbol-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe to, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to 24-hour ticker updates for COIN-M symbols. Use the symbol-type-aware volume properties on the callback model.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#individual-symbol-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe to, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to the merged USDⓈ-M and COIN-M 24-hour ticker stream. Use <c>SymbolType</c> and the product-safe volume properties.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#all-market-tickers-streams" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamTick>>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to real-time book-ticker updates for the provided symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#individual-symbol-book-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to real-time book-ticker updates for the provided symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#individual-symbol-book-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to real-time book-ticker updates for the merged UM and CM market
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#all-book-tickers-stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to liquidation-order snapshots for a specific symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#market-liquidation-order-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to liquidation-order snapshots for the provided symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#market-liquidation-order-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to merged USDⓈ-M and COIN-M all-market liquidation-order snapshots
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#all-market-liquidation-order-streams" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to contract/symbol updates
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#contract-info-stream" /></para>
    /// </summary>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToSymbolUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamSymbolUpdate>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to standard partial order-book depth updates for the provided symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#partial-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to subscribe on, for example `BTCUSD_PERP`</param>
    /// <param name="levels">The amount of entries to be returned in the update, 5, 10 or 20</param>
    /// <param name="updateInterval">Explicit update interval in milliseconds, either 100 or 500. Null uses the default 250 milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to standard partial order-book depth updates for the provided symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#partial-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to subscribe on, for example `BTCUSD_PERP`</param>
    /// <param name="levels">The amount of entries to be returned in each update, 5, 10 or 20</param>
    /// <param name="updateInterval">Explicit update interval in milliseconds, either 100 or 500. Null uses the default 250 milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to standard diff order-book depth updates for the provided symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#diff-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="updateInterval">Explicit update interval in milliseconds, either 100 or 500. Null uses the default 250 milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribes to standard diff order-book depth updates for the provided symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-streams/~#diff-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
    /// <param name="updateInterval">Explicit update interval in milliseconds, either 100 or 500. Null uses the default 250 milliseconds</param>
    /// <param name="onMessage">The event handler for the received data</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage, CancellationToken ct = default);

}
