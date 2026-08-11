namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options REST API Client Account Methods
/// </summary>
public interface IBinanceOptionsRestClientAccount
{
    /// <summary>
    /// Get current Option Margin account information.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/account#option-margin-account-information" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. Binance defaults to 5000 milliseconds when omitted and currently publishes no maximum for this endpoint</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The current Option Margin account information</returns>
    Task<RestCallResult<BinanceOptionsMarginAccount>> GetMarginAccountAsync(long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query account funding flows.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/account#account-funding-flow" /></para>
    /// </summary>
    /// <param name="currency">Asset type; the current endpoint supports USDT</param>
    /// <param name="recordId">Return the recordId and subsequent data, the latest data is returned by default, e.g 100000</param>
    /// <param name="startTime">Start Time, e.g 1593511200000</param>
    /// <param name="endTime">End Time, e.g 1593512200000</param>
    /// <param name="limit">Number of result sets returned Default:100 Max:1000</param>
    /// <param name="receiveWindow">The receive window for which this request is active. Binance currently publishes no maximum for this endpoint</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsAccountFundingFlow>>> GetAccountFundingFlowAsync(BinanceOptionsFundingFlowCurrency currency, long? recordId = null, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, long? receiveWindow = null, CancellationToken ct = default);
}
