namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client
/// </summary>
public interface IBinanceMarginRestApiClient :
    IBinanceMarginRestApiClientAccount,
    IBinanceMarginRestApiClientBorrowRepay,
    IBinanceMarginRestApiClientMarketData,
    IBinanceMarginRestApiClientRiskDataStream,
    IBinanceMarginRestApiClientTrade,
    IBinanceMarginRestApiClientTradeDataStream,
    IBinanceMarginRestApiClientTransfer;