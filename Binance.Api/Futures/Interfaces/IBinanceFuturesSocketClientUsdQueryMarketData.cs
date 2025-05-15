namespace Binance.Api.Futures;

/// <summary>
/// Binance USD Futures Market Data Web Socket Query API
/// </summary>
public interface IBinanceFuturesSocketClientUsdQueryMarketData
{
    /// <summary>
    /// Gets the order book for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/websocket-api" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the order book for, for example `ETHUSDT`</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    Task<CallResult<BinanceFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the price of a symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/websocket-api/Symbol-Price-Ticker" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Price of symbol</returns>
    Task<CallResult<BinanceFuturesPrice>> GetPriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets the price of all symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/websocket-api/Symbol-Price-Ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Price of symbol</returns>
    Task<CallResult<List<BinanceFuturesPrice>>> GetPricesAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets the best price/quantity on the order book for a symbol.
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/websocket-api/Symbol-Order-Book-Ticker" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get book price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    Task<CallResult<BinanceFuturesBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets the best price/quantity on the order book for all symbols.
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/market-data/websocket-api/Symbol-Order-Book-Ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    Task<CallResult<List<BinanceFuturesBookTicker>>> GetBookPricesAsync(CancellationToken ct = default);
}