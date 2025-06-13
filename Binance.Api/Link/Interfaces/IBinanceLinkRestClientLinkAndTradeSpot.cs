namespace Binance.Api.Link;

/// <summary>
/// Interface for the Binance Link and Trade (Spot) Rest API client.
/// </summary>
public interface IBinanceLinkRestClientLinkAndTradeSpot
{
    /// <summary>
    /// Query Client If The New User.
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/spot" /></para>
    /// </summary>
    /// <param name="apiAgentCode">Api Agent Code</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceLinkSpotIfNewUser>> GetIfNewUserAsync(string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Customize Id For Client (USER DATA) （For Partner）
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/spot/Customize-Id-For-Client-For-Partner" /></para>
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="customerId">Customer Id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceLinkSpotCustomerIdPartner>> SetCustomerIdByPartnerAsync(string email, string customerId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Client Email Customized Id (USER DATA) （For Partner）
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/spot/Get-Client-Email-Customized-Id" /></para>
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="customerId">Customer Id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceLinkSpotCustomerIdPartner>>> GetCustomerIdByPartnerAsync(string? email = null, string? customerId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Customize Id For Client (USER DATA)(For client)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/spot/Customize-Id-For-Client-For-Client" /></para>
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="apiAgentCode">Api Agent Code</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceLinkSpotCustomerIdClient>> SetCustomerIdByClientAsync(string customerId, string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get User’s Customize Id (USER DATA)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/spot/Get-User-Customize-Id" /></para>
    /// </summary>
    /// <param name="apiAgentCode">Api Agent Code</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceLinkSpotCustomerIdClient>> GetCustomerIdByClientAsync(string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Rebate Recent Record （USER DATA）(For Partner)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/spot/Query-Rebate-Recent-Record-For-Partner" /></para>
    /// </summary>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="customerId">Customer Id</param>
    /// <param name="limit">Limit. Max:500</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceLinkSpotRebatePartner>>> GetRebateHistoryByPartnerAsync(DateTime startTime, DateTime endTime, string? customerId = null, int limit = 100, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Rebate Recent Record(For Client)
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/spot/Query-Rebate-Recent-Record-For-Client" /></para>
    /// </summary>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Limit. Max:500</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceLinkSpotRebateClient>>> GetRebateHistoryByClientAsync(DateTime? startTime = null, DateTime? endTime = null, int limit = 100, int? receiveWindow = null, CancellationToken ct = default);
}