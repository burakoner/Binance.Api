namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options REST API Client Account Methods
/// </summary>
public interface IBinanceOptionsRestClientAccount
{
    /// <summary>
    /// UNDOCUMENTED: Calls <c>GET /eapi/v1/account</c>, which is absent from the current Options catalog and official connector. Its server lifecycle is unresolved; use <see cref="GetMarginAccountAsync"/> for the current documented margin-account contract
    /// </summary>
    /// <param name="receiveWindow">Wrapper receive-window parameter; the current server contract is undocumented</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default);

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
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/account/Account-Funding-Flow" /></para>
    /// </summary>
    /// <param name="currency">Asset type, only support USDT as of now</param>
    /// <param name="recordId">Return the recordId and subsequent data, the latest data is returned by default, e.g 100000</param>
    /// <param name="startTime">Start Time, e.g 1593511200000</param>
    /// <param name="endTime">End Time, e.g 1593512200000</param>
    /// <param name="limit">Number of result sets returned Default:100 Max:1000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsAccountFundingFlow>>> GetAccountFundingFlowAsync(string currency, long? recordId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// UNDOCUMENTED: Calls <c>GET /eapi/v1/income/asyn</c>, which is absent from the current Options catalog and official connector. Its server lifecycle is unresolved and it is not a supported current contract
    /// </summary>
    /// <param name="startTime">Wrapper start-time parameter; the current server contract is undocumented</param>
    /// <param name="endTime">Wrapper end-time parameter; the current server contract is undocumented</param>
    /// <param name="receiveWindow">Wrapper receive-window parameter; the current server contract is undocumented</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsDownloadId>> GetTransactionHistoryDownloadIdAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// UNDOCUMENTED: Calls <c>GET /eapi/v1/income/asyn/id</c>, which is absent from the current Options catalog and official connector. Its server lifecycle is unresolved and it is not a supported current contract
    /// </summary>
    /// <param name="downloadId">Wrapper download identifier parameter; the current server contract is undocumented</param>
    /// <param name="receiveWindow">Wrapper receive-window parameter; the current server contract is undocumented</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsDownloadLink>> GetTransactionHistoryDownloadLinkAsync(long downloadId, int? receiveWindow = null, CancellationToken ct = default);
}
