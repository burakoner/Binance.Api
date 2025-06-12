namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options REST API Client Account Methods
/// </summary>
public interface IBinanceOptionsRestClientAccount
{
    /// <summary>
    /// Get current account information.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/account" /></para>
    /// </summary>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default);

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
    /// Get download id for option transaction history
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/account/Get-Download-Id-For-Option-Transaction-History" /></para>
    /// </summary>
    /// <param name="startTime">Start Time, e.g 1593511200000</param>
    /// <param name="endTime">End Time, e.g 1593512200000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsDownloadId>> GetTransactionHistoryDownloadIdAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get option transaction history download Link by Id
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/account/Get-Option-Transaction-History-Download-Link-by-Id" /></para>
    /// </summary>
    /// <param name="downloadId">get by download id api</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsDownloadLink>> GetTransactionHistoryDownloadLinkAsync(long downloadId, int? receiveWindow = null, CancellationToken ct = default);
}