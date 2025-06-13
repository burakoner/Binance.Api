namespace Binance.Api.Rebate;

/// <summary>
/// Interface for the Binance Rebate Rest API client.
/// </summary>
public interface IBinanceRebateRestClient
{
    /// <summary>
    /// Get Spot Rebate History Records
    /// <para><a href="https://developers.binance.com/docs/rebate/rest-api" /></para>
    /// </summary>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="page">Page</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRebateContainer<BinanceRebateSpotRecord>>> GetSpotRebateHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? receiveWindow = null, CancellationToken ct = default);
}