﻿namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures USD market data endpoints
/// </summary>
public interface IBinanceFuturesRestClientUsdMarketData
{
    /// <summary>
    /// Pings the Binance Futures API
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if successful ping, false if no response</returns>
    Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default);

    /// <summary>
    /// Requests the server for the local time. This function also determines the offset between server and local time and uses this for subsequent API calls
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Check-Server-Time" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Server time</returns>
    Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default);

    /// <summary>
    /// Get's information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Exchange-Information" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Exchange info</returns>
    Task<RestCallResult<BinanceFuturesUsdExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets the order book for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Order-Book" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the order book for, for example `ETHUSDT`</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The order book for the symbol</returns>
    Task<RestCallResult<BinanceFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get the most recent trades for a symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Recent-Trades-List" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get trades for, for example `ETHUSDT`</param>
    /// <param name="limit">Max amount of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesUsdTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get trade history for a symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Old-Trades-Lookup" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get trades for, for example `ETHUSDT`</param>
    /// <param name="limit">The max amount of results, max 500</param>
    /// <param name="fromId">Return trades after this trade id</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesUsdTrade>>> GetHistoricalTradesAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default);

    /// <summary>
    /// Gets compressed, aggregate trades. Trades that fill at the time, from the same order, with the same price will have the quantity aggregated.
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Compressed-Aggregate-Trades-List" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the trades for, for example `ETHUSDT`</param>
    /// <param name="fromId">ID to get aggregate trades from INCLUSIVE.</param>
    /// <param name="startTime">Time to start getting trades from</param>
    /// <param name="endTime">Time to stop getting trades from</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The aggregated trades list for the symbol</returns>
    Task<RestCallResult<List<BinanceFuturesAggregatedTrade>>> GetAggregatedTradesAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get klines for a symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Kline-Candlestick-Data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="interval">The kline interval</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesUsdKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get candlestick data for the provided pair
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Continuous-Contract-Kline-Candlestick-Data" /></para>
    /// </summary>
    /// <param name="pair">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="contractType">The contract type</param>
    /// <param name="interval">The candlestick timespan</param>
    /// <param name="startTime">Start time to get candlestick data</param>
    /// <param name="endTime">End time to get candlestick data</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The candlestick data for the provided symbol</returns>
    Task<RestCallResult<List<BinanceFuturesUsdKline>>> GetContinuousContractKlinesAsync(string pair, BinanceFuturesContractType contractType, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get Kline/candlestick data for the index price of a pair.
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Index-Price-Kline-Candlestick-Data" /></para>
    /// </summary>
    /// <param name="pair">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="interval">The candlestick timespan</param>
    /// <param name="startTime">Start time to get candlestick data</param>
    /// <param name="endTime">End time to get candlestick data</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The candlestick data for the provided symbol</returns>
    Task<RestCallResult<List<BinanceFuturesKline>>> GetIndexPriceKlinesAsync(string pair, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Kline/candlestick bars for the mark price of a symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Mark-Price-Kline-Candlestick-Data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol get the data for, for example `ETHUSDT`</param>
    /// <param name="interval">The interval of the klines</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="startTime">Start time</param>
    /// <param name="endTime">End time</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesKline>>> GetMarkPriceKlinesAsync(string symbol, BinanceKlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

    /// <summary>
    /// Get premium index klines for a symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Premium-Index-Kline-Data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="interval">The kline interval</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesKline>>> GetPremiumIndexKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get Mark Price and Funding Rate for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Mark-Price" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    Task<RestCallResult<BinanceFuturesMarkPrice>> GetMarkPriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get Mark Price and Funding Rate for all symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Mark-Price" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    Task<RestCallResult<List<BinanceFuturesMarkPrice>>> GetMarkPricesAsync(CancellationToken ct = default);

    /// <summary>
    /// Get funding rate history for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Get-Funding-Rate-History" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="startTime">Start time to get funding rate history</param>
    /// <param name="endTime">End time to get funding rate history</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The funding rate history for the provided symbol</returns>
    Task<RestCallResult<List<BinanceFuturesFundingRate>>> GetFundingRatesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get funding rate info for symbols that had FundingRateCap/ FundingRateFloor / fundingIntervalHours adjustment
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Get-Funding-Rate-Info" /></para>
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesFundingInfo>>> GetFundingInfoAsync(CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours change
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/24hr-Ticker-Price-Change-Statistics" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    Task<RestCallResult<BinanceFuturesUsdTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get data regarding the last 24 hours change
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/24hr-Ticker-Price-Change-Statistics" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    Task<RestCallResult<List<BinanceFuturesUsdTicker>>> GetTickersAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets the price of a symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Symbol-Price-Ticker" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Price of symbol</returns>
    Task<RestCallResult<BinanceFuturesPrice>> GetPriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get a list of the prices of all symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Symbol-Price-Ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of prices</returns>
    Task<RestCallResult<List<BinanceFuturesPrice>>> GetPricesAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets the best price/quantity on the order book for a symbol.
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Symbol-Order-Book-Ticker" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get book price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    Task<RestCallResult<BinanceFuturesBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets the best price/quantity on the order book.
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Symbol-Order-Book-Ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    Task<RestCallResult<List<BinanceFuturesBookTicker>>> GetBookPricesAsync(CancellationToken ct = default);

    /// <summary>
    /// Get present open interest of a specific symbol.
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Open-Interest" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Open Interest info</returns>
    Task<RestCallResult<BinanceFuturesOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets Open Interest History
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Open-Interest-Statistics" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="period">The period timespan</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="startTime">Start time to get open interest history</param>
    /// <param name="endTime">End time to get open interest history</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Open Interest History info</returns>
    Task<RestCallResult<List<BinanceFuturesOpenInterestHistory>>> GetOpenInterestHistoryAsync(string symbol, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);
    /// <summary>
    /// Gets Top Trader Long/Short Ratio (Positions)
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Top-Trader-Long-Short-Ratio" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="period">The period timespan</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="startTime">Start time to get top trader long/short ratio (positions)</param>
    /// <param name="endTime">End time to get top trader long/short ratio (positions)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Top Trader Long/Short Ratio (Positions) info</returns>    
    Task<RestCallResult<List<BinanceFuturesLongShortRatio>>> GetTopLongShortPositionRatioAsync(string symbol, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

    /// <summary>
    /// Gets Top Trader Long/Short Ratio (Accounts)
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Top-Long-Short-Account-Ratio" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="period">The period timespan</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="startTime">Start time to get top trader long/short ratio (accounts)</param>
    /// <param name="endTime">End time to get top trader long/short ratio (accounts)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Top Trader Long/Short Ratio (Accounts) info</returns>
    Task<RestCallResult<List<BinanceFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbol, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

    /// <summary>
    /// Gets Global Long/Short Ratio (Accounts)
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Long-Short-Ratio" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="period">The period timespan</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="startTime">Start time to get global long/short ratio (accounts)</param>
    /// <param name="endTime">End time to get global long/short ratio (accounts)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Global Long/Short Ratio (Accounts) info</returns>
    Task<RestCallResult<List<BinanceFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbol, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

    /// <summary>
    /// Gets Taker Buy/Sell Volume Ratio
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Taker-BuySell-Volume" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="period">The period timespan</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="startTime">Start time to get taker buy/sell volume ratio</param>
    /// <param name="endTime">End time to get taker buy/sell volume ratio</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Taker Buy/Sell Volume Ratio info</returns>
    Task<RestCallResult<List<BinanceFuturesBuySellVolumeRatio>>> GetTakerBuySellVolumeRatioAsync(string symbol, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);
    
    /// <summary>
    /// Get basis data
    /// <para><a href="https://binance-docs.github.io/apidocs/futures/en/#basis" /></para>
    /// </summary>
    /// <param name="pair">The pair to get the data for, for example `ETHUSDT`</param>
    /// <param name="contractType">The contract type</param>
    /// <param name="period">The period timespan</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="startTime">Start time</param>
    /// <param name="endTime">End time</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesBasis>>> GetBasisAsync(string pair, BinanceFuturesContractType contractType, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

    /// <summary>
    /// Gets composite index info
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Composite-Index-Symbol-Information" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesCompositeIndexInfo>>> GetCompositeIndexInfoAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Get asset index for Multi-Assets mode for a symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Multi-Assets-Mode-Asset-Index" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesAssetIndex>> GetAssetIndexAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get asset indexes for Multi-Assets mode for all symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/rest-api/Multi-Assets-Mode-Asset-Index" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesAssetIndex>>> GetAssetIndexesAsync(CancellationToken ct = default);

    // TODO: Query Index Price Constituents
    // TODO: Query Insurance Fund Balance Snapshot
}