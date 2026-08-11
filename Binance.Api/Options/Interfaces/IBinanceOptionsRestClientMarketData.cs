namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options REST API Client Market Data Methods
/// </summary>
public interface IBinanceOptionsRestClientMarketData
{
    /// <summary>
    /// 24 hour rolling window price change statistics.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#ticker24hr-price-change-statistics" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsTicker>>> GetTickersAsync(CancellationToken ct = default);

    /// <summary>
    /// 24 hour rolling window price change statistics.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#ticker24hr-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsTicker>> GetTickersAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get historical exercise records.
    /// REALISTIC_VALUE_STRICKEN -> Exercised
    /// EXTRINSIC_VALUE_EXPIRED -> Expired OTM
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#historical-exercise-records" /></para>
    /// </summary>
    /// <param name="underlying">Underlying index like BTCUSDT</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Number of records Default:100 Max:100</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsPublicExercise>>> GetPublicExerciseRecordsAsync(string? underlying = null, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get open interest for specific underlying asset on specific expiration date.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#open-interest" /></para>
    /// </summary>
    /// <param name="underlying">Underlying index like BTCUSDT</param>
    /// <param name="expiration">expiration date, e.g 221225</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsOpenInterest>>> GetOpenInterestAsync(string underlying, DateTime expiration, CancellationToken ct = default);

    /// <summary>
    /// Check orderbook depth on specific symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#order-book" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="limit">Default:100. Allowed values: 5, 10, 20, 50, 100, 500, 1000</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsOrderBook>> GetOrderBookAsync(string symbol, long? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get recent market trades
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#recent-trades-list" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="limit">Number of records Default:100 Max:500</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsPublicTrade>>> GetRecentTradesAsync(string symbol, long? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get recent block trades
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#recent-block-trades-list" /></para>
    /// </summary>
    /// <param name="symbol">Optional option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="limit">Number of records Default:100 Max:500</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsBlockTrade>>> GetRecentBlockTradesAsync(string? symbol = null, long? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get spot index price for option underlying.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#index-price" /></para>
    /// </summary>
    /// <param name="underlying">Spot pair（Option contract underlying asset, e.g BTCUSDT)</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsIndexPrice>> GetIndexPriceAsync(string underlying, CancellationToken ct = default);

    /// <summary>
    /// Kline/candlestick bars for an option symbol. Klines are uniquely identified by their open time.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#kline-candlestick-data" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="interval">Time interval</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Number of records Default:500 Max:1500</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Option mark price and greek info.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#option-mark-price" /></para>
    /// </summary>
    /// <param name="symbol">Optional option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsMarkPrice>>> GetMarkPriceAsync(string? symbol = null, CancellationToken ct = default);
}
