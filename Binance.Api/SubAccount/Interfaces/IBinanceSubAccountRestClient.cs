namespace Binance.Api.SubAccount;

/// <summary>
/// Interface for the Binance Sub-Account Rest API client.
/// </summary>
public interface IBinanceSubAccountRestClient :
    IBinanceSubAccountRestClientAccount,
    IBinanceSubAccountRestClientApi,
    IBinanceSubAccountRestClientAsset,
    IBinanceSubAccountRestClientManaged;
