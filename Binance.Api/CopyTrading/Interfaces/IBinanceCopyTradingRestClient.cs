namespace Binance.Api.CopyTrading;

/// <summary>
/// Interface for the Binance Copy Trading Rest API client.
/// </summary>
public interface IBinanceCopyTradingRestClient
{
    /// <summary>
    /// Interface for the Binance Copy Trading Futures Rest API client.
    /// </summary>
    IBinanceCopyTradingRestClientFutures Futures { get; }
}