namespace Binance.Api.InstitutionalLoan;

/// <summary>
/// Interface for the Binance Institutional Loan Rest API client.
/// </summary>
public interface IBinanceInstitutionalLoanRestClient
{
    /// <summary>
    /// Query Institution Loan Group Details which is executed by parent account
    /// <para><a href="https://developers.binance.com/docs/institutional_loan/account" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceInstitutionalLoanGroup>>> GetLoanGroupsAsync(CancellationToken ct = default);

    /// <summary>
    /// Query One Institution Loan Group Details which is executed by credit account
    /// <para><a href="https://developers.binance.com/docs/institutional_loan/account/Query-One-Institution-Loan-Group_Detail" /></para>
    /// </summary>
    /// <param name="groupId">Group Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceInstitutionalLoanGroup>> GetLoanGroupAsync(long groupId, CancellationToken ct = default);
}