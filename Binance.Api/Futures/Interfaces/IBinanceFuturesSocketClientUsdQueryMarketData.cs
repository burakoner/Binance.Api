namespace Binance.Api.Futures;

/// <summary>
/// Binance USD Futures Market Data Web Socket Query API
/// </summary>
public interface IBinanceFuturesSocketClientUsdQueryMarketData
{
    /// <summary>
    /// Gets the standard order book for the provided symbol. Retail Price Improvement (RPI) orders are excluded from the response
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/market-data#order-book" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the order book for, for example `ETHUSDT`</param>
    /// <param name="limit">Max number of results. Valid values are 5, 10, 20, 50, 100, 500, and 1000; defaults to 500</param>
    /// <param name="ct">Cancellation token</param>
    Task<CallResult<BinanceFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the price of a symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/market-data#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Price of symbol</returns>
    Task<CallResult<BinanceFuturesPrice>> GetPriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets the price of all symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/market-data#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Price of symbol</returns>
    Task<CallResult<List<BinanceFuturesPrice>>> GetPricesAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets the best price/quantity on the standard order book for a symbol. Retail Price Improvement (RPI) orders are excluded from the response.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/market-data#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get book price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Book price</returns>
    Task<CallResult<BinanceFuturesWebSocketBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets the best price/quantity on the standard order book for all symbols. Retail Price Improvement (RPI) orders are excluded from the response.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/market-data#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    Task<CallResult<List<BinanceFuturesWebSocketBookTicker>>> GetBookPricesAsync(CancellationToken ct = default);
}
