namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options Market Maker Block Trade REST API Client
/// </summary>
public interface IBinanceOptionsRestClientMarketMakerBlockTrade
{
    /// <summary>
    /// Send in a new block trade order.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/market-maker-block-trade" /></para>
    /// </summary>
    /// <param name="liquidity">Liquidity</param>
    /// <param name="legs">Max 1 (only single leg supported)</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> PlaceBlockOrderAsync(BinanceOptionsLiquidity liquidity, IEnumerable<BinanceOptionsMarketMakerBlockOrderLeg> legs, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel a block trade order.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/market-maker-block-trade/Cancel-Block-Trade-Order" /></para>
    /// </summary>
    /// <param name="blockOrderMatchingKey">Block Order Matching Key</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> CancelBlockOrderAsync(string blockOrderMatchingKey, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Extends a block trade expire time by 30 mins from the current time.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/market-maker-block-trade/Extend-Block-Trade-Order" /></para>
    /// </summary>
    /// <param name="blockOrderMatchingKey">Block Order Matching Key</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> ExtendBlockOrderAsync(string blockOrderMatchingKey, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Check block trade order status.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/market-maker-block-trade/Query-Block-Trade-Order" /></para>
    /// </summary>
    /// <param name="blockOrderMatchingKey">Block Order Matching Key</param>
    /// <param name="underlying">Underlying</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsMarketMakerBlockOrder>>> GetBlockOrderAsync(string? blockOrderMatchingKey = null, string? underlying = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Accept a block trade order
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/market-maker-block-trade/Accept-Block-Trade-Order" /></para>
    /// </summary>
    /// <param name="blockOrderMatchingKey">Block Order Matching Key</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> AcceptBlockOrderAsync(string blockOrderMatchingKey, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query block trade details; returns block trade details from counterparty's perspective.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/market-maker-block-trade/Query-Block-Trade-Detail" /></para>
    /// </summary>
    /// <param name="blockOrderMatchingKey">Block Order Matching Key</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> GetBlockOrderAsync(string blockOrderMatchingKey, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets block trades for a specific account.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/market-maker-block-trade/Account-Block-Trade-List" /></para>
    /// </summary>
    /// <param name="underlying">Underlying</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsMarketMakerBlockTrade>>> GetBlockTradesAsync(string? underlying = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default);
}