namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client Market Data Methods
/// </summary>
public interface IBinanceSpotRestClientMarketData
{
    /// <summary>
    /// Gets the order book for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#order-book" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the order book for, for example `ETHUSDT`</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The order book for the symbol</returns>
    Task<RestCallResult<BinanceSpotOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the recent trades for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#recent-trades-list" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get recent trades for, for example `ETHUSDT`</param>
    /// <param name="limit">Result limit</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of recent trades</returns>
    Task<RestCallResult<List<BinanceSpotTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the historical trades for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#old-trade-lookup" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get recent trades for, for example `ETHUSDT`</param>
    /// <param name="limit">Result limit</param>
    /// <param name="fromId">From which trade id on results should be retrieved</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of recent trades</returns>
    Task<RestCallResult<List<BinanceSpotTrade>>> GetHistoricalTradesAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default);

    /// <summary>
    /// Gets compressed, aggregate trades. Trades that fill at the same time, from the same order, with the same price will have the quantity aggregated.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#compressedaggregate-trades-list" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the trades for, for example `ETHUSDT`</param>
    /// <param name="fromId">ID to get aggregate trades from INCLUSIVE.</param>
    /// <param name="startTime">Time to start getting trades from</param>
    /// <param name="endTime">Time to stop getting trades from</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The aggregated trades list for the symbol</returns>
    Task<RestCallResult<List<BinanceSpotAggregatedTrade>>> GetAggregatedTradesAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get candlestick data for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#klinecandlestick-data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="interval">The candlestick timespan</param>
    /// <param name="startTime">Start time to get candlestick data</param>
    /// <param name="endTime">End time to get candlestick data</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The candlestick data for the provided symbol</returns>
    Task<RestCallResult<List<BinanceSpotKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get candlestick data for the provided symbol. Returns modified kline data, optimized for the presentation of candlestick charts
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#uiklines" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="interval">The candlestick timespan</param>
    /// <param name="startTime">Start time to get candlestick data</param>
    /// <param name="endTime">End time to get candlestick data</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The candlestick data for the provided symbol</returns>
    Task<RestCallResult<List<BinanceSpotKline>>> GetUIKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets current average price for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#current-average-price" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSpotAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    Task<RestCallResult<BinanceSpotTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    Task<RestCallResult<List<BinanceSpotTicker>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    Task<RestCallResult<List<BinanceSpotTicker>>> GetTickersAsync(CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    Task<RestCallResult<BinanceSpotMiniTicker>> GetMiniTickerAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    Task<RestCallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    Task<RestCallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSpotTradingDayTicker>> GetTradingDayTickerAsync(string symbol, string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceTradingDayMiniTicker>> GetTradingDayMiniTickerAsync(string symbol, string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(string? timeZone = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the price of a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Price of symbol</returns>
    Task<RestCallResult<BinanceSpotPriceTicker>> GetPriceTickerAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    ///  Gets the prices of symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to get the price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of prices</returns>
    Task<RestCallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

    /// <summary>
    ///  Gets the prices of symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of prices</returns>
    Task<RestCallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets the best price/quantity on the order book for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get book price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    Task<RestCallResult<BinanceSpotBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets the best price/quantity on the order book for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Symbols to get book price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    Task<RestCallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);

    /// <summary>
    /// Gets the best price/quantity on the order book for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    Task<RestCallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(CancellationToken ct = default);

    /// <summary>
    /// Get data based on the last x time, specified as windowSize
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#rolling-window-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">The symbols to get data for, for example `ETHUSDT`</param>
    /// <param name="windowSize">The window size to use</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSpotTicker>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, CancellationToken ct = default);

    /// <summary>
    /// Get data based on the last x time, specified as windowSize
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#rolling-window-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to get data for, for example `ETHUSDT`</param>
    /// <param name="windowSize">The window size to use</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceSpotTicker>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, CancellationToken ct = default);
}