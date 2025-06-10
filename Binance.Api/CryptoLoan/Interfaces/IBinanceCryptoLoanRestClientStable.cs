namespace Binance.Api.CryptoLoan;

/// <summary>
/// Interface for the Binance Algo REST API Crypto Loan > Stable Rate Methods
/// </summary>
public interface IBinanceCryptoLoanRestClientStable :
    IBinanceCryptoLoanRestClientStableMarketData,
    IBinanceCryptoLoanRestClientStableTrade,
    IBinanceCryptoLoanRestClientStableUser;