namespace Binance.Api.CopyTrading;

/// <summary>
/// Interface for the Binance Copy Trading Futures Rest API client.
/// </summary>
public interface IBinanceCopyTradingRestClientFutures
{
    /// <summary>
    /// Get Futures Lead Trader Status
    /// <para><a href="https://developers.binance.com/docs/copy_trading/future-copy-trading" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceCopyTradingFuturesLeadTraderStatus>> GetLeadTraderStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Futures Lead Trading Symbol Whitelist
    /// <para><a href="https://developers.binance.com/docs/copy_trading/future-copy-trading/Get-Futures-Lead-Trading-Symbol-Whitelist" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceCopyTradingFuturesLeadTradingSymbol>>> GetLeadTradingSymbolsAsync(int? receiveWindow = null, CancellationToken ct = default);
}