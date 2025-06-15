namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures Data -> Market Data endpoints
/// </summary>
public interface IBinanceFuturesRestClientDataMarketData
{
    /// <summary>
    /// Get Future TickLevel Orderbook Historical Data Download Link
    /// <para><a href="https://developers.binance.com/docs/derivatives/futures-data/market-data" /></para>
    /// </summary>
    /// <param name="symbol">symbol name, e.g. BTCUSDT or BTCUSD_PERP ｜</param>
    /// <param name="type">T_DEPTH for ticklevel orderbook data, S_DEPTH for orderbook snapshot data</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesDataLink>>> GetFuturesDataLinkAsync(string symbol, BinanceFuturesDataType type, DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default);
}
