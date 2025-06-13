namespace Binance.Api.Link;

/// <summary>
/// Interface for the Binance Link and Trade (Futures) Rest API client.
/// </summary>
public interface IBinanceLinkRestClientLinkAndTradeFutures
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
    Task<RestCallResult<BinanceLinkFuturesIfNewUser>> GetIfNewUserAsync(string brokerId, BinanceFuturesType? type = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Customize Id For Client (USER DATA)(For Partner)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Customize-Id-For-Client-For-Partner" /></para>
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="customerId">Customer Id</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceLinkFuturesCustomerIdPartner>> SetCustomerIdByPartnerAsync(string email, string customerId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Client Email Customized Id (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-Client-Email-Customized-Id" /></para>
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="customerId">Customer Id</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceLinkFuturesCustomerIdPartner>>> GetCustomerIdByPartnerAsync(string? email = null, string? customerId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Customize Id For Client (USER DATA)(For client)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Customize-Id-For-Client-For-Client" /></para>
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="apiAgentCode">API Agent Code</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceLinkFuturesCustomerIdClient>> SetCustomerIdByClientAsync(string customerId, string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get User’s Customize Id (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-User-Customize-Id" /></para>
    /// </summary>
    /// <param name="apiAgentCode">API Agent Code</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceLinkFuturesCustomerIdClient>> GetCustomerIdByClientAsync(string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<RestCallResult<List<BinanceLinkFuturesIncomeRecord>>> GetIncomeHistoryAsync(string? symbol = null, BinanceLinkIncomeType? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<RestCallResult<List<BinanceLinkFuturesTraderNumber>>> GetTraderNumberAsync(BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Rebate Data Overview (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/futures/Get-Rebate-Data-Overview" /></para>
    /// </summary>
    /// <param name="type">Futures Type</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceLinkFuturesRebateOverview>> GetRebateDataOverviewAsync(BinanceFuturesType? type = null, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<RestCallResult<List<BinanceLinkFuturesTradeVolume>>> GetUserTradeVolumeAsync(BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<RestCallResult<List<BinanceLinkFuturesRebateVolume>>> GetUserRebateVolumeAsync(BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<RestCallResult<List<BinanceLinkFuturesTraderDetail>>> GetTraderDetailsAsync(string? customerId = null, BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}