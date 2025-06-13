namespace Binance.Api.Pay;

/// <summary>
/// Interface for the Binance Pay History Rest API client.
/// </summary>
public interface IBinancePayRestClientHistory
{
    /// <summary>
    /// Get Spot Rebate History Records
    /// <para><a href="https://developers.binance.com/docs/rebate/rest-api" /></para>
    /// </summary>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Limit</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinancePayHistoryTransaction>>> GetHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}