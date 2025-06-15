namespace Binance.Api.C2C;

/// <summary>
/// Interface for the Binance C2C Rest API client.
/// </summary>
public interface IBinanceC2CRestClient
{
    /// <summary>
    /// Get C2C Trade History
    /// <para><a href="https://developers.binance.com/docs/c2c/rest-api" /></para>
    /// </summary>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="page">Page</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceDataTotalResponse<BinanceC2CUserOrderRecord>>> GetHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? receiveWindow = null, CancellationToken ct = default);
}