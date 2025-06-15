namespace Binance.Api.Fiat;

/// <summary>
/// Interface for the Binance Fiat Rest API client.
/// </summary>
public interface IBinanceFiatRestClient
{
    /// <summary>
    /// Get Fiat Deposit History
    /// <para><a href="https://developers.binance.com/docs/fiat/rest-api" /></para>
    /// </summary>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="page">Page</param>
    /// <param name="rows">Rows</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceDataTotalResponse<BinanceFiatTransaction>>> GetDepositHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? rows = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Fiat Withdrawal History
    /// <para><a href="https://developers.binance.com/docs/fiat/rest-api" /></para>
    /// </summary>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="page">Page</param>
    /// <param name="rows">Rows</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceDataTotalResponse<BinanceFiatTransaction>>> GetWithdrawalHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? rows = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Fiat Deposit/Withdraw History
    /// <para><a href="https://developers.binance.com/docs/fiat/rest-api/Get-Fiat-Payments-History" /></para>
    /// </summary>
    /// <param name="type">Buy or Sell</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="page">Page</param>
    /// <param name="rows">Rows</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceDataTotalResponse<BinanceFiatPayment>>> GetPaymentHistoryAsync(BinanceFiatPaymentType type, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? rows = null, int? receiveWindow = null, CancellationToken ct = default);
}