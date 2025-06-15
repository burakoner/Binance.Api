namespace Binance.Api.SimpleEarn;

/// <summary>
/// Interface for the Binance Locked Simple Earn -> Account Rest API client.
/// </summary>
public interface IBinanceSimpleEarnRestClientLockedAccount
{
    /// <summary>
    /// Get Simple Earn account info
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#simple-account-user_data" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get a list of simple earn locked products
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-simple-earn-locked-product-list-user_data" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnLockedProduct>>> GetProductsAsync(string? asset = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get locked product position info
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-locked-product-position-user_data" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset</param>
    /// <param name="positionId">Filter by position id</param>
    /// <param name="projectId">Filter by project id</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnLockedPosition>>> GetPositionsAsync(string? asset = null, string? positionId = null, string? projectId = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get locked product personal quota left
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-locked-personal-left-quota-user_data" /></para>
    /// </summary>
    /// <param name="projectId">Project id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnQuota>> GetQuotaAsync(string projectId, int? receiveWindow = null, CancellationToken ct = default);
}