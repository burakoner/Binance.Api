namespace Binance.Api.VipLoan;

/// <summary>
/// Interface for the Binance VIP Loan REST API Client
/// </summary>
public interface IBinanceVipLoanRestClient :
    IBinanceVipLoanRestClientMarketData,
    IBinanceVipLoanRestClientTrade,
    IBinanceVipLoanRestClientUser;