namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options Market Maker Endpoints REST API Client
/// </summary>
public interface IBinanceOptionsRestClientMarketMakerAccount
{
    /// <summary>
    /// Get config for MMP.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-endpoints#get-market-maker-protection-config" /></para>
    /// </summary>
    /// <param name="underlying">Underlying</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerProtection>> GetProtectionAsync(string underlying, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// This endpoint returns the auto-cancel parameters for each underlying symbol. Note only active auto-cancel parameters will be returned, if countdownTime is set to 0 (ie. countdownTime has been turned off), the underlying symbol and corresponding countdownTime parameter will not be returned in the response.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-endpoints#get-auto-cancel-all-open-orders" /></para>
    /// </summary>
    /// <param name="underlying">Optional underlying filter</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerAutoCancelAll>> GetCancelAllCountdownAsync(string? underlying = null, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Set config for MMP. Market Maker Protection(MMP) is a set of protection mechanism for option market maker, this mechanism is able to prevent mass trading in short period time. Once market maker's account branches the threshold, the Market Maker Protection will be triggered. When Market Maker Protection triggers, all the current MMP orders will be canceled, new MMP orders will be rejected. Market maker can use this time to reevaluate market and modify order price.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-endpoints#set-market-maker-protection-config" /></para>
    /// </summary>
    /// <param name="underlying">Underlying</param>
    /// <param name="windowTimeInMilliseconds">MMP interval in milliseconds; range [0, 5000]</param>
    /// <param name="frozenTimeInMilliseconds">MMP frozen time in milliseconds, if set to 0 manual reset is required</param>
    /// <param name="quantityLimit"></param>
    /// <param name="deltaLimit"></param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerProtection>> SetProtectionAsync(string underlying, long windowTimeInMilliseconds, long frozenTimeInMilliseconds, decimal quantityLimit, decimal deltaLimit, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// This endpoint resets the time from which the countdown will begin to the time this messaged is received. It should be called repeatedly as heartbeats. Multiple heartbeats can be updated at once by specifying the underlying symbols as a list (ex. BTCUSDT,ETHUSDT) in the underlyings parameter.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-endpoints#auto-cancel-all-open-orders" /></para>
    /// </summary>
    /// <param name="underlyings">Underlyings</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerUnderlyings>> CancelAllCountdownHeartbeatAsync(IEnumerable<string> underlyings, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Reset MMP, start MMP order again.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-endpoints#reset-market-maker-protection-config" /></para>
    /// </summary>
    /// <param name="underlying">Underlying</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerProtection>> ResetProtectionAsync(string underlying, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// This endpoint sets the parameters of the auto-cancel feature which cancels all open orders (both market maker protection and non market maker protection order types) of the underlying symbol at the end of the specified countdown time period if no heartbeat message is sent. After the countdown time period, all open orders will be cancelled and new orders will be rejected with error code -2010 until either a heartbeat message is sent or the auto-cancel feature is turned off by setting countdownTime to 0.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-maker-endpoints#set-auto-cancel-all-open-orders" /></para>
    /// </summary>
    /// <param name="underlying">Underlying</param>
    /// <param name="countdownTime">Countdown time in milliseconds (ex. 1,000 for 1 second). 0 to disable the timer. Negative values (ex. -10000) are not accepted. Minimum acceptable value is 5,000 </param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsMarketMakerCountdown>> SetCancelAllCountdownAsync(string underlying, long countdownTime, long? receiveWindow = null, CancellationToken ct = default);
}
