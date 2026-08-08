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
    /// <param name="status">Optional required symbol status</param>
    Task<CallResult<BinanceSpotOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the recent trades for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#recent-trades" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<List<BinanceSpotTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the historical trades for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#historical-trades" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="fromId">Filter by from trade id</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<List<BinanceSpotTrade>>> GetHistoricalTradesAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets historical block trades.
    /// </summary>
    Task<CallResult<List<BinanceSpotBlockTrade>>> GetHistoricalBlockTradesAsync(string symbol, long fromId, int? limit = null, CancellationToken ct = default);

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
    Task<CallResult<List<BinanceSpotAggregatedTrade>>> GetAggregatedTradesAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

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
    /// <param name="timeZone">Kline timezone from -12:00 through +14:00</param>
    Task<CallResult<List<BinanceSpotKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, string? timeZone = null, int? limit = null, CancellationToken ct = default);

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
    /// <param name="timeZone">Kline timezone from -12:00 through +14:00</param>
    Task<CallResult<List<BinanceSpotKline>>> GetUIKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, string? timeZone = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the current average price for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#current-average-price" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets the current reference price for a symbol.
    /// </summary>
    Task<CallResult<BinanceSpotReferencePrice>> GetReferencePriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets the reference-price calculation configuration for a symbol.
    /// </summary>
    Task<CallResult<BinanceSpotReferencePriceCalculation>> GetReferencePriceCalculationAsync(string symbol, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional required symbol status</param>
    Task<CallResult<BinanceSpotTicker>> GetTickerAsync(string symbol, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceSpotTicker>>> GetTickersAsync(IEnumerable<string> symbols, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceSpotTicker>>> GetTickersAsync(BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional required symbol status</param>
    Task<CallResult<BinanceSpotMiniTicker>> GetMiniTickerAsync(string symbol, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(IEnumerable<string> symbols, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/market-data-requests#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get Price change statistics for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="timeZone">Default: 0 (UTC)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional required symbol status</param>
    Task<CallResult<BinanceSpotTradingDayTicker>> GetTradingDayTickerAsync(string symbol, string? timeZone = null, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get Price change statistics for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="timeZone">Default: 0 (UTC)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(IEnumerable<string> symbols, string? timeZone = null, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get Price change statistics for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="timeZone">Default: 0 (UTC)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional required symbol status</param>
    Task<CallResult<BinanceTradingDayMiniTicker>> GetTradingDayMiniTickerAsync(string symbol, string? timeZone = null, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get Price change statistics for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="timeZone">Default: 0 (UTC)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(IEnumerable<string> symbols, string? timeZone = null, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

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
    /// <param name="status">Optional required symbol status</param>
    Task<CallResult<BinanceSpotRollingWindowTicker>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

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
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceSpotRollingWindowTicker>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Gets mini rolling-window statistics for one symbol.
    /// </summary>
    Task<CallResult<BinanceSpotMiniTicker>> GetRollingWindowMiniTickerAsync(string symbol, TimeSpan? windowSize = null, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Gets mini rolling-window statistics for multiple symbols.
    /// </summary>
    Task<CallResult<List<BinanceSpotMiniTicker>>> GetRollingWindowMiniTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get the latest market price for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Query price for a single symbol</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional required symbol status</param>
    Task<CallResult<BinanceSpotPriceTicker>> GetPriceTickerAsync(string symbol, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get the latest market price for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Query price for multiple symbols</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(IEnumerable<string> symbols, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get the latest market price for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get the current best price and quantity on the order book.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Query ticker for a single symbol</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional required symbol status</param>
    Task<CallResult<BinanceSpotBookTicker>> GetBookTickerAsync(string symbol, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get the current best price and quantity on the order book.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Query ticker for multiple symbols</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(IEnumerable<string> symbols, BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);

    /// <summary>
    /// Get the current best price and quantity on the order book.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/websocket-api/market-data-requests#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <param name="status">Optional symbol-status filter</param>
    Task<CallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(BinanceSpotSymbolStatusFilter? status = null, CancellationToken ct = default);
}
