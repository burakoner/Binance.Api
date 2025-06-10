namespace Binance.Api.Broker;

/// <summary>
/// Interface for the Binance Link and Trade (Futures) Rest API client.
/// </summary>
public interface IBinanceBrokerRestClientLinkAndTradeFutures
{
    /// <summary>
    /// Query Client If The New User (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures" /></para>
    /// </summary>
    /// <param name="brokerId">Broker Id</param>
    /// <param name="type">Futures Type</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceBrokerFuturesIfNewUser>> GetIfNewUserAsync(string brokerId, BinanceFuturesType? type = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Customize Id For Client (USER DATA)(For Partner)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Customize-Id-For-Client-For-Partner" /></para>
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="customerId">Customer Id</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceBrokerFuturesCustomerIdPartner>> SetCustomerIdByPartnerAsync(string email, string customerId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Client Email Customized Id (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-Client-Email-Customized-Id" /></para>
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="customerId">Customer Id</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceBrokerFuturesCustomerIdPartner>>> GetCustomerIdByPartnerAsync(string? email = null, string? customerId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Customize Id For Client (USER DATA)(For client)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Customize-Id-For-Client-For-Client" /></para>
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="apiAgentCode">API Agent Code</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceBrokerFuturesCustomerIdClient>> SetCustomerIdByClientAsync(string customerId, string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get User’s Customize Id (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-User-Customize-Id" /></para>
    /// </summary>
    /// <param name="apiAgentCode">API Agent Code</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceBrokerFuturesCustomerIdClient>> GetCustomerIdByClientAsync(string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Income History(USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-Income-History" /></para>
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="incomeType">Income Type</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">default 500, max 1000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceBrokerFuturesIncomeRecord>>> GetIncomeHistoryAsync(string? symbol = null, BinanceBrokerIncomeType? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Trader Number (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-Trader-Number" /></para>
    /// </summary>
    /// <param name="type">Futures Type</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">default 500, max 1000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceBrokerFuturesTraderNumber>>> GetTraderNumberAsync(BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Rebate Data Overview (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-Rebate-Data-Overview" /></para>
    /// </summary>
    /// <param name="type">Futures Type</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceBrokerFuturesRebateOverview>> GetRebateDataOverviewAsync(BinanceFuturesType? type = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get User Trade Volume (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-User-Trade-Volume" /></para>
    /// </summary>
    /// <param name="type">Futures Type</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">default 500, max 1000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceBrokerFuturesTradeVolume>>> GetUserTradeVolumeAsync(BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Rebate Volume (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-Rebate-Volume" /></para>
    /// </summary>
    /// <param name="type">Futures Type</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">default 500, max 1000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceBrokerFuturesRebateVolume>>> GetUserRebateVolumeAsync(BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Trader Detail (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-Trader-Detail" /></para>
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="type">Futures Type</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">default 500, max 1000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceBrokerFuturesTraderDetail>>> GetTraderDetailsAsync(string? customerId = null, BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}