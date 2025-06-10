namespace Binance.Api.Broker;

/// <summary>
/// Interface for the Binance Link and Trade (Portfolio Margin) Rest API client.
/// </summary>
public interface IBinanceBrokerRestClientLinkAndTradePortfolioMargin
{
    /// <summary>
    /// Query Client If The New User (USER DATA)(PAPI)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/portfolio-margin" /></para>
    /// </summary>
    /// <param name="brokerId">Broker Id</param>
    /// <param name="type">Futures Type</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceBrokerPortfolioMarginIfNewUser>> GetIfNewUserAsync(string brokerId, BinanceFuturesType? type = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Customize Id For Client (USER DATA)(For client)(PAPI)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Customize-Id-For-Client-For-Client-PAPI" /></para>
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="apiAgentCode">API Agent Code</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceBrokerFuturesCustomerIdClient>> SetCustomerIdByClientAsync(string customerId, string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get User’s Customize Id (USER DATA)(PAPI)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-User-Customize-Id-PAPI" /></para>
    /// </summary>
    /// <param name="apiAgentCode">API Agent Code</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceBrokerFuturesCustomerIdClient>> GetCustomerIdByClientAsync(string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default);
}