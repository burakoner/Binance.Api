namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Trade Data Stream Methods
/// </summary>
public interface IBinanceMarginRestApiClientTradeDataStream
{
    Task<RestCallResult<string>> StartMarginUserStreamAsync(CancellationToken ct = default);
    Task<RestCallResult<bool>> KeepAliveMarginUserStreamAsync(string listenKey, CancellationToken ct = default);
    Task<RestCallResult<bool>> StopMarginUserStreamAsync(string listenKey, CancellationToken ct = default);
    Task<RestCallResult<string>> StartIsolatedMarginUserStreamAsync(string symbol, CancellationToken ct = default);
    Task<RestCallResult<bool>> KeepAliveIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default);
    Task<RestCallResult<bool>> CloseIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default);
}