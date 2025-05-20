namespace Binance.Api.SimpleEarn;

/// <summary>
/// Interface for the Binance Locked Simple Earn -> History Rest API client.
/// </summary>
public interface IBinanceSimpleEarnRestClientLockedHistory
{
    /// <summary>
    /// Get Simple Earn locked product subscription records
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-locked-subscription-record-user_data" /></para>
    /// </summary>
    /// <param name="purchaseId">Filter by purchase id</param>
    /// <param name="asset">Filler by asset</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceQueryRecords<BinanceSimpleEarnLockedRecord>>> GetSubscriptionsAsync(string? purchaseId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Simple Earn locked product redemption records
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-locked-redemption-record-user_data" /></para>
    /// </summary>
    /// <param name="positionId">Filter by position id</param>
    /// <param name="redeemId">Filler by redeem id</param>
    /// <param name="asset">Filler by asset</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceQueryRecords<BinanceSimpleEarnLockedRedemptionRecord>>> GetRedemptionsAsync(string? positionId = null, string? redeemId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Simple Earn locked product reward records
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-locked-rewards-history-user_data" /></para>
    /// </summary>
    /// <param name="positionId">Position id</param>
    /// <param name="asset">Filler by asset</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceQueryRecords<BinanceSimpleEarnLockedRewardRecord>>> GetRewardsAsync(string? positionId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);
}