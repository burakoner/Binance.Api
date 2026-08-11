namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options Web Socket API Client Market Data Stream Methods
/// </summary>
public interface IBinanceOptionsSocketClientStreamMarketData
{
    /// <summary>
    /// New symbol listing stream.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/New-Symbol-Info" /></para>
    /// </summary>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToNewSymbolsAsync(Action<WebSocketDataEvent<BinanceOptionsStreamSymbol>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Option open interest for specific underlying asset on specific expiration date. E.g.ETH@openInterest@221125
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/Open-Interest" /></para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="expiration">Expiration Date</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOpenInterestAsync(string asset, DateTime expiration, Action<WebSocketDataEvent<BinanceOptionsStreamOpenInterest>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Option open interest for specific underlying asset on specific expiration date. E.g.ETH@openInterest@221125
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/Open-Interest" /></para>
    /// </summary>
    /// <param name="tuples">Asset &amp; Expiration Date Tuple List</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOpenInterestAsync(IEnumerable<(string UnderlyingAsset, DateTime ExpirationDate)> tuples, Action<WebSocketDataEvent<BinanceOptionsStreamOpenInterest>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// The mark price for all option symbols on specific underlying asset. E.g.ETH@markPrice
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/Mark-Price" /></para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceAsync(string asset, Action<WebSocketDataEvent<BinanceOptionsStreamMarkPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// The mark price for all option symbols on specific underlying asset. E.g.ETH@markPrice
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/Mark-Price" /></para>
    /// </summary>
    /// <param name="assets">Assets</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceAsync(IEnumerable<string> assets, Action<WebSocketDataEvent<BinanceOptionsStreamMarkPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// The Kline/Candlestick Stream push updates to the current klines/candlestick every 1000 milliseconds (if existing).
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="interval">Interval</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceOptionsStreamKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// The Kline/Candlestick Stream push updates to the current klines/candlestick every 1000 milliseconds (if existing).
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="intervals">Intervals</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceOptionsStreamKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// The Kline/Candlestick Stream push updates to the current klines/candlestick every 1000 milliseconds (if existing).
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbols">Symbols</param>
    /// <param name="interval">Interval</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceOptionsStreamKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// The Kline/Candlestick Stream push updates to the current klines/candlestick every 1000 milliseconds (if existing).
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/Kline-Candlestick-Streams" /></para>
    /// </summary>
    /// <param name="symbols">Symbols</param>
    /// <param name="intervals">Intervals</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceOptionsStreamKline>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// 24-hour ticker information for an underlying symbol and expiration date. E.g. btcusdt@optionTicker251230
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#hour24-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Underlying symbol</param>
    /// <param name="expiration">Expiration</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, DateTime expiration, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// 24-hour ticker information for underlying symbols and expiration dates.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#hour24-ticker" /></para>
    /// </summary>
    /// <param name="tuples">Underlying symbol and expiration date tuples</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<(string Symbol, DateTime ExpirationDate)> tuples, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Underlying(e.g ETHUSDT) index stream.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/Index-Price-Streams" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPricesAsync(string symbol, Action<WebSocketDataEvent<BinanceOptionsStreamIndexPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Underlying(e.g ETHUSDT) index stream.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/websocket-market-streams/Index-Price-Streams" /></para>
    /// </summary>
    /// <param name="symbols">Symbols</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPricesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceOptionsStreamIndexPrice>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// 24-hour ticker information for an underlying symbol without an expiration filter.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#hour24-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// 24-hour ticker information for underlying symbols without expiration filters.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#hour24-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Symbols</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Push raw trade information for a specific Options symbol or underlying symbol. E.g. btcusdt@optionTrade
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#trade-streams" /></para>
    /// </summary>
    /// <param name="symbol">Symbol or Underlying Asset</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceOptionsStreamTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Push raw trade information for specific Options symbols or underlying symbols.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#trade-streams" /></para>
    /// </summary>
    /// <param name="symbols">Symbols or Underlying Assets</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceOptionsStreamTrade>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to diff order book updates for a specific symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#diff-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="updateInterval">100ms or 500ms</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(string symbol, int updateInterval, Action<WebSocketDataEvent<BinanceOptionsStreamOrderBook>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to diff order book updates for specific symbols.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#diff-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbols">Symbols</param>
    /// <param name="updateInterval">100ms or 500ms</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(IEnumerable<string> symbols, int updateInterval, Action<WebSocketDataEvent<BinanceOptionsStreamOrderBook>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to best bid and ask updates for a specific symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#individual-symbol-book-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceOptionsStreamBookTicker>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to best bid and ask updates for specific symbols.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#individual-symbol-book-ticker-streams" /></para>
    /// </summary>
    /// <param name="symbols">Symbols</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceOptionsStreamBookTicker>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to partial order book updates for a specific symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#partial-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="levels">Valid levels are 5, 10, 20</param>
    /// <param name="updateInterval">100ms or 500ms</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(string symbol, int levels, int updateInterval, Action<WebSocketDataEvent<BinanceOptionsStreamOrderBook>> onMessage, CancellationToken ct = default);

    /// <summary>
    /// Subscribe to partial order book updates for a specific symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/ws-streams/public#partial-book-depth-streams" /></para>
    /// </summary>
    /// <param name="symbols">Symbols</param>
    /// <param name="levels">Valid levels are 5, 10, 20</param>
    /// <param name="updateInterval">100ms or 500ms</param>
    /// <param name="onMessage">On Data Handler</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(IEnumerable<string> symbols, int levels, int updateInterval, Action<WebSocketDataEvent<BinanceOptionsStreamOrderBook>> onMessage, CancellationToken ct = default);
}
