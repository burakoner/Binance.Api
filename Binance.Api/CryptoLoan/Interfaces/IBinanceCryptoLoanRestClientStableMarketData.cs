namespace Binance.Api.CryptoLoan;

/// <summary>
/// Interface for the Binance Algo REST API Crypto Loan > Stable Rate -> Market Data Methods
/// </summary>
public interface IBinanceCryptoLoanRestClientStableMarketData
{
    /// <summary>
    /// Get income history from crypto loans
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-crypto-loans-income-history-user_data" /></para>
    /// </summary>
    /// <param name="asset">The asset</param>
    /// <param name="type">Filter by type of incoming</param>
    /// <param name="startTime">Filter by startTime from</param>
    /// <param name="endTime">Filter by endTime from</param>
    /// <param name="limit">Limit of the amount of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceCryptoLoanStableIncome>>> GetIncomeHistoryAsync(string asset, BinanceCryptoLoanStableIncomeType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}
