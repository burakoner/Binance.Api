namespace Binance.Api.CryptoLoan;

/// <summary>
/// Interface for the Binance Algo REST API Client
/// </summary>
public interface IBinanceCryptoLoanRestClient
{
    /// <summary>
    /// Flexible Rate REST API Client
    /// </summary>
    IBinanceCryptoLoanRestClientFlexible Flexible { get; }

    /// <summary>
    /// Stable Rate REST API Client
    /// </summary>
    IBinanceCryptoLoanRestClientStable Stable { get; }
}
