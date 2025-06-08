namespace Binance.Api.CryptoLoan;

/// <summary>
/// Interface for the Binance Algo REST API Crypto Loan > Flexible Rate Methods
/// </summary>
public interface IBinanceCryptoLoanRestClientFlexible :
    IBinanceCryptoLoanRestClientFlexibleMarketData,
    IBinanceCryptoLoanRestClientFlexibleTrade,
    IBinanceCryptoLoanRestClientFlexibleUser;