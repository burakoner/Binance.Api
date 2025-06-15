namespace Binance.Api.SimpleEarn;

/// <summary>
/// Interface for the Binance Flexible Simple Earn -> History Rest API client.
/// </summary>
public interface IBinanceSimpleEarnRestClientFlexibleHistory
{
    /// <summary>
    /// Get Simple Earn flexible product subscription records 
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-flexible-subscription-record-user_data" /></para>
    /// </summary>
    /// <param name="productId">Filter by product id</param>
    /// <param name="purchaseId">Filter by purchase id</param>
    /// <param name="asset">Filler by asset</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleRecord>>> GetSubscriptionsAsync(string? productId = null, string? purchaseId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Simple Earn flexible product redemption records
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-flexible-redemption-record-user_data" /></para>
    /// </summary>
    /// <param name="productId">Filter by product id</param>
    /// <param name="redeemId">Filler by redeem id</param>
    /// <param name="asset">Filler by asset</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleRedemptionRecord>>> GetRedemptionsAsync(string? productId = null, string? redeemId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Simple Earn flexible product reward records
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-flexible-rewards-history-user_data" /></para>
    /// </summary>
    /// <param name="type">Type or rewards</param>
    /// <param name="productId">Filter by product id</param>
    /// <param name="asset">Filler by asset</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleRewardRecord>>> GetRewardsAsync(BinanceSimpleEarnRewardType type, string? productId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get collateral records
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-rate-history-user_data" /></para>
    /// </summary>
    /// <param name="productId">Product id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleCollateralRecord>>> GetCollateralsAsync(string productId, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get rate history
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-rate-history-user_data" /></para>
    /// </summary>
    /// <param name="productId">Product id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleRateRecord>>> GetRatesAsync(string productId, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);
}