namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options Market Maker Block Trade REST API Client
/// </summary>
public interface IBinanceOptionsRestClientMarketMakerBlockTrade
{
    /// <summary>
    /// Send in a new block trade order.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-block-trade#new-block-trade-order" /></para>
    /// </summary>
    /// <param name="liquidity">Liquidity</param>
    /// <param name="legs">Max 1 (only single leg supported)</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> PlaceBlockOrderAsync(BinanceOptionsLiquidity liquidity, IEnumerable<BinanceOptionsMarketMakerBlockOrderRequestLeg> legs, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel a block trade order.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-block-trade#cancel-block-trade-order" /></para>
    /// </summary>
    /// <param name="blockOrderMatchingKey">Block Order Matching Key</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> CancelBlockOrderAsync(string blockOrderMatchingKey, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Extends a block trade expire time by 30 mins from the current time.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-block-trade#extend-block-trade-order" /></para>
    /// </summary>
    /// <param name="blockOrderMatchingKey">Block Order Matching Key</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> ExtendBlockOrderAsync(string blockOrderMatchingKey, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query block trade orders.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-block-trade#query-block-trade-order" /></para>
    /// </summary>
    /// <param name="blockOrderMatchingKey">Block Order Matching Key</param>
    /// <param name="underlying">Underlying</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsMarketMakerBlockOrder>>> GetBlockOrdersAsync(string? blockOrderMatchingKey = null, string? underlying = null, DateTime? startTime = null, DateTime? endTime = null, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Accept a block trade order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-block-trade#accept-block-trade-order" /></para>
    /// </summary>
    /// <param name="blockOrderMatchingKey">Block Order Matching Key</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> AcceptBlockOrderAsync(string blockOrderMatchingKey, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query block trade details; returns block trade details from counterparty's perspective.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-block-trade#query-block-trade-details" /></para>
    /// </summary>
    /// <param name="blockOrderMatchingKey">Block Order Matching Key</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> GetBlockTradeDetailsAsync(string blockOrderMatchingKey, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets block trades for a specific account.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-block-trade#account-block-trade-list" /></para>
    /// </summary>
    /// <param name="underlying">Underlying</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsMarketMakerBlockTrade>>> GetBlockTradesAsync(string? underlying = null, DateTime? startTime = null, DateTime? endTime = null, long? receiveWindow = null, CancellationToken ct = default);
}
