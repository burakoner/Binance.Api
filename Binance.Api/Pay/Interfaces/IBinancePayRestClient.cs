namespace Binance.Api.Pay;

/// <summary>
/// Interface for the Binance Pay Rest API client.
/// </summary>
public interface IBinancePayRestClient
{
    /// <summary>
    /// Binance Pay History REST API Client
    /// </summary>
    IBinancePayRestClientHistory History { get; }

    /// <summary>
    /// Binance Pay Merchant REST API Client
    /// </summary>
    IBinancePayRestClientMerchant Merchant { get; }
}