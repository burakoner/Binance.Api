namespace Binance.Api.DualInvestment;

/// <summary>
/// Interface for the Binance Dual Investment Rest API client.
/// </summary>
public interface IBinanceDualInvestmentRestClient :
    IBinanceDualInvestmentRestClientMarketData,
    IBinanceDualInvestmentRestClientTrade;