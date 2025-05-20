namespace Binance.Api.SimpleEarn;

/// <summary>
/// Interface for the Binance Locked Simple Earn -> Earn Rest API client.
/// </summary>
public interface IBinanceSimpleEarnRestClientLockedEarn
{
    /// <summary>
    /// Subscribe to locked product
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#subscribe-locked-product-trade" /></para>
    /// </summary>
    /// <param name="projectId">Project id</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="autoSubscribe">Auto subscribe, default true</param>
    /// <param name="sourceAccount">Source account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnLockedPurchase>> SubscribeAsync(string projectId, decimal quantity, bool? autoSubscribe = null, BinanceSimpleEarnSourceAccount? sourceAccount = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Redeem locked product
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#redeem-locked-product-trade" /></para>
    /// </summary>
    /// <param name="positionId">Position id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnLockedRedemption>> RedeemAsync(string positionId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Set locked product auto subscribe toggle
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#set-locked-auto-subscribe-user_data" /></para>
    /// </summary>
    /// <param name="positionId">Position id</param>
    /// <param name="autoSubscribe">Auto subscribe enabled or not</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnResult>> SetAutoSubscribeAsync(string positionId, bool autoSubscribe, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get locked subscription preview
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-locked-subscription-preview-user_data" /></para>
    /// </summary>
    /// <param name="projectId">Project id</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="autoSubscribe">Auto subscribe</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceSimpleEarnLockedPreview>>> GetSubscriptionPreviewAsync(string projectId, decimal quantity, bool? autoSubscribe = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Set redeem option for Locked product
    /// <para><a href="https://developers.binance.com/docs/simple_earn/earn/Set-Locked-Redeem-Option" /></para>
    /// </summary>
    /// <param name="positionId">Position id</param>
    /// <param name="redeemTo">Redeem to</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSimpleEarnResult>> SetRedeemOptionAsync(string positionId, BinanceSimpleEarnRedeemOption redeemTo, int? receiveWindow = null, CancellationToken ct = default);
}