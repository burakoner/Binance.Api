namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot Web Socket API Client Market Data Methods
/// </summary>
public interface IBinanceSpotSocketClientQueryMarketData
{
    /// <summary>
    /// Gets the order book for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#order-book" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="limit">Number of entries</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the recent trades for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#recent-trades" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the historical trades for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#historical-trades" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="fromId">Filter by from trade id</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotTrade>>> GetTradeHistoryAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets compressed, aggregate trades. Trades that fill at the same time, from the same order, with the same price will have the quantity aggregated.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#aggregate-trades" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="fromId">Filter by from trade id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceStreamAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get candlestick data for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#klines" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="interval">Kline interval</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get candlestick data for the provided symbol. Returns modified kline data, optimized for the presentation of candlestick charts
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#ui-klines" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="interval">Kline interval</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotKline>>> GetUIKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the current average price for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#current-average-price" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotTicker>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotTicker>>> GetTickersAsync(CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotMiniTicker>> GetMiniTickerAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotMiniTicker>>> GetMiniTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotMiniTicker>>> GetMiniTickersAsync(CancellationToken ct = default);

    /// <summary>
    /// Get Price change statistics for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="timeZone">Default: 0 (UTC)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotTradingDayTicker>> GetTradingDayTickerAsync(string symbol, string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get Price change statistics for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="timeZone">Default: 0 (UTC)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get Price change statistics for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="timeZone">Default: 0 (UTC)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get Price change statistics for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="timeZone">Default: 0 (UTC)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotTradingDayTicker>> GetTradingDayMiniTickerAsync(string symbol, string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get Price change statistics for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="timeZone">Default: 0 (UTC)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotTradingDayTicker>>> GetTradingDayMiniTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get Price change statistics for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="timeZone">Default: 0 (UTC)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotTradingDayTicker>>> GetTradingDayMiniTickersAsync(string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get rolling window price change statistics with a custom window.
    /// This request is similar to ticker.24hr, but statistics are computed on demand using the arbitrary window you specify.
    /// Note: Window size precision is limited to 1 minute.While the closeTime is the current time of the request, openTime always start on a minute boundary.As such, the effective window might be up to 59999 ms wider than the requested windowSize.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#rolling-window-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">Query ticker of a single symbol</param>
    /// <param name="windowSize">Default 1d</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotTicker>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, CancellationToken ct = default);

    /// <summary>
    /// Get rolling window price change statistics with a custom window.
    /// This request is similar to ticker.24hr, but statistics are computed on demand using the arbitrary window you specify.
    /// Note: Window size precision is limited to 1 minute.While the closeTime is the current time of the request, openTime always start on a minute boundary.As such, the effective window might be up to 59999 ms wider than the requested windowSize.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#rolling-window-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">Query ticker for multiple symbols</param>
    /// <param name="windowSize">Default 1d</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotTicker>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, CancellationToken ct = default);

    /// <summary>
    /// Get the latest market price for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Query price for a single symbol</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotPriceTicker>> GetPriceTickerAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get the latest market price for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Query price for multiple symbols</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotPriceTicker>>> GetPriceTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

    /// <summary>
    /// Get the latest market price for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotPriceTicker>>> GetPriceTickersAsync(CancellationToken ct = default);

    /// <summary>
    /// Get the current best price and quantity on the order book.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Query ticker for a single symbol</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get the current best price and quantity on the order book.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Query ticker for multiple symbols</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotBookTicker>>> GetBookTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

    /// <summary>
    /// Get the current best price and quantity on the order book.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotBookTicker>>> GetBookTickersAsync(CancellationToken ct = default);
}