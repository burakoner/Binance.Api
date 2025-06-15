namespace Binance.Api.SimpleEarn;

/// <summary>
/// Interface for the Binance Flexible Simple Earn -> Account Rest API client.
/// </summary>
public interface IBinanceSimpleEarnRestClientFlexibleAccount
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
    /// Get a list of simple earn flexible products
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-simple-earn-flexible-product-list-user_data" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleProduct>>> GetProductsAsync(string? asset = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get flexible product position info
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-flexible-product-position-user_data" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset</param>
    /// <param name="productId">Filter by product id</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexiblePosition>>> GetPositionsAsync(string? asset = null, string? productId = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get flexible product personal quota left
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-flexible-personal-left-quota-user_data" /></para>
    /// </summary>
    /// <param name="productId">Product id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnQuota>> GetQuotaAsync(string productId, int? receiveWindow = null, CancellationToken ct = default);
}