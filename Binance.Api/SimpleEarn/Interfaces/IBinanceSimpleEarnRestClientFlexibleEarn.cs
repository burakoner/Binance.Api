namespace Binance.Api.SimpleEarn;

/// <summary>
/// Interface for the Binance Flexible Simple Earn -> Earn Rest API client.
/// </summary>
public interface IBinanceSimpleEarnRestClientFlexibleEarn
{
    /// <summary>
    /// Subscribe to flexible product
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#subscribe-flexible-product-trade" /></para>
    /// </summary>
    /// <param name="productId">Product id</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="autoSubscribe">Auto subscribe, default true</param>
    /// <param name="sourceAccount">Source account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnFlexiblePurchase>> SubscribeAsync(string productId, decimal quantity, bool? autoSubscribe = null, BinanceSimpleEarnSourceAccount? sourceAccount = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Redeem flexible product
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#redeem-flexible-product-trade" /></para>
    /// </summary>
    /// <param name="productId">Product id</param>
    /// <param name="redeemAll">Whether to redeem all. If not then quantity should be specified</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="destinationAccount">Destination account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnFlexibleRedemption>> RedeemAsync(string productId, bool? redeemAll = null, decimal? quantity = null, BinanceSimpleEarnSourceAccount? destinationAccount = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Set flexible product auto subscribe toggle
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#set-flexible-auto-subscribe-user_data" /></para>
    /// </summary>
    /// <param name="productId">Product id</param>
    /// <param name="autoSubscribe">Auto subscribe enabled or not</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnResult>> SetAutoSubscribeAsync(string productId, bool autoSubscribe, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get flexible subscription preview
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-flexible-subscription-preview-user_data" /></para>
    /// </summary>
    /// <param name="productId">Product id</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnFlexiblePreview>> GetSubscriptionPreviewAsync(string productId, decimal quantity, int? receiveWindow = null, CancellationToken ct = default);
}