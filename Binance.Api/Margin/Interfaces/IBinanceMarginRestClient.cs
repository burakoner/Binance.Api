namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client
/// </summary>
public interface IBinanceMarginRestClient :
    IBinanceMarginRestClientAccount,
    IBinanceMarginRestClientBorrowRepay,
    IBinanceMarginRestClientMarketData,
    IBinanceMarginRestClientRiskDataStream,
    IBinanceMarginRestClientTrade,
    IBinanceMarginRestClientTradeDataStream,
    IBinanceMarginRestClientTransfer;