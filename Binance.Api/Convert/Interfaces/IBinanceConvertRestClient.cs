namespace Binance.Api.Convert;

/// <summary>
/// Interface for the Binance Convert Rest API client.
/// </summary>
public interface IBinanceConvertRestClient :
    IBinanceConvertRestClientMarketData,
    IBinanceConvertRestClientTrade;