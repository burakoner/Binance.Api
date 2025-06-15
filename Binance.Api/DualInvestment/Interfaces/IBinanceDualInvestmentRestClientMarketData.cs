using Binance.Api.Options;

namespace Binance.Api.DualInvestment;

/// <summary>
/// Interface for the Binance Dual Investment -> Market Data Rest API client.
/// </summary>
public interface IBinanceDualInvestmentRestClientMarketData
{
    /// <summary>
    /// Get Dual Investment product list
    /// <para><a href="https://developers.binance.com/docs/dual_investment/market-data" /></para>
    /// </summary>
    /// <param name="side">Input CALL or PUT</param>
    /// <param name="exercisedCoin">Target exercised asset, e.g.: if you subscribe to a high sell product (call option), you should input: optionType:CALL,exercisedCoin:USDT,investCoin:BNB; if you subscribe to a low buy product (put option), you should input: optionType:PUT,exercisedCoin:BNB,investCoin:USDT</param>
    /// <param name="investCoin">Asset used for subscribing, e.g.: if you subscribe to a high sell product (call option), you should input: optionType:CALL,exercisedCoin:USDT,investCoin:BNB; if you subscribe to a low buy product (put option), you should input: optionType:PUT,exercisedCoin:BNB,investCoin:USDT</param>
    /// <param name="pageSize">Default: 10, Maximum: 100</param>
    /// <param name="pageIndex">Default: 1</param>
    /// <param name="receiveWindow">Receive Window. The value cannot be greater than 60000</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceListTotalResponse<BinanceDualInvestmentProduct>>> GetProductsAsync(BinanceOptionsSide side, string exercisedCoin, string investCoin, int? pageSize = null, int? pageIndex = null, int? receiveWindow = null, CancellationToken ct = default);
}