namespace Binance.Api.Link;

/// <summary>
/// Interface for the Binance Exchange Link Rest API client.
/// </summary>
public interface IBinanceLinkRestClientExchangeLink:
    IBinanceLinkRestClientExchangeLinkAccount,
    IBinanceLinkRestClientExchangeLinkAsset,
    IBinanceLinkRestClientExchangeLinkFee;