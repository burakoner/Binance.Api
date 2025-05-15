namespace Binance.Api.Broker;

/// <summary>
/// Interface for the Binance Exchange Link Rest API client.
/// </summary>
public interface IBinanceBrokerRestClientExchangeLink:
    IBinanceBrokerRestClientExchangeLinkAccount,
    IBinanceBrokerRestClientExchangeLinkAsset,
    IBinanceBrokerRestClientExchangeLinkFee;