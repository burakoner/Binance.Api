namespace Binance.Api.AutoInvest;

/// <summary>
/// Interface for the Binance Auto Invest Rest API client.
/// </summary>
public interface IBinanceAutoInvestRestClient :
    IBinanceAutoInvestRestClientMarketData,
    IBinanceAutoInvestRestClientTrade;