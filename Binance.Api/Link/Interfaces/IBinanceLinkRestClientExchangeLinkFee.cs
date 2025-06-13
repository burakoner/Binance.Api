namespace Binance.Api.Link;

/// <summary>
/// Interface for the Binance Exchange Link - Fee Rest API client.
/// </summary>
public interface IBinanceLinkRestClientExchangeLinkFee
{
    /// <summary>
    /// Change Sub Account Commission
    /// <para>This request will change the commission for a sub account</para>
    /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
    /// <para>If margin disabled, it is not allowed to send marginMakerCommission or marginTakerCommission</para>
    /// <para>If margin enabled, marginMakerCommission or marginTakerCommission has default value as spotMakerCommission or spotTakerCommission</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="makerCommission">Maker commission</param>
    /// <param name="takerCommission">Taker commission</param>
    /// <param name="marginMakerCommission">Margin maker commission</param>
    /// <param name="marginTakerCommission">Margin taker commission</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Sub account commission result</returns>
    Task<RestCallResult<BinanceLinkSubAccountCommission>> SetCommissionAsync(string subAccountId, decimal makerCommission, decimal takerCommission, decimal? marginMakerCommission = null, decimal? marginTakerCommission = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Change Sub Account USDT-Ⓜ Futures Commission Adjustment
    /// <para>This request will change the USDT-Ⓜ futures commission for a sub account</para>
    /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
    /// <para>The sub-account's USDT-Ⓜ futures commission of a symbol equals to the base commission of the symbol on the sub-account's fee tier plus the commission adjustment</para>
    /// <para>If futures disabled, it is not allowed to set subaccount's USDT-Ⓜ futures commission adjustment on any symbol</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="symbol">Symbol</param>
    /// <param name="makerAdjustment">Maker adjustment (100 for 0.01%)</param>
    /// <param name="takerAdjustment">Taker adjustment (100 for 0.01%)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Sub account USDT-Ⓜ futures commission result</returns>
    Task<RestCallResult<BinanceLinkSubAccountFuturesCommission>> SetFuturesCommissionAdjustmentAsync(string subAccountId, string symbol, int makerAdjustment, int takerAdjustment, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Sub Account USDT-Ⓜ Futures Commission Adjustment
    /// <para>The sub-account's USDT-Ⓜ futures commission of a symbol equals to the base commission of the symbol on the sub-account's fee tier plus the commission adjustment</para>
    /// <para>If symbol not sent, commission adjustment of all symbols will be returned</para>
    /// <para>If futures disabled, it is not allowed to set subaccount's USDT-Ⓜ futures commission adjustment on any symbol</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="symbol">Symbol</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Sub account USDT-Ⓜ futures commissions result</returns>
    Task<RestCallResult<List<BinanceLinkSubAccountFuturesCommission>>> GetFuturesCommissionAdjustmentAsync(string subAccountId, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Change Sub Account COIN-Ⓜ Futures Commission Adjustment
    /// <para>This request will change the COIN-Ⓜ futures commission for a sub account</para>
    /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
    /// <para>The sub-account's COIN-Ⓜ futures commission of a symbol equals to the base commission of the symbol on the sub-account's fee tier plus the commission adjustment</para>
    /// <para>If futures disabled, it is not allowed to set subaccount's COIN-Ⓜ futures commission adjustment on any symbol</para>
    /// <para>Different symbols have the same commission for the same pair</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="pair">Pair</param>
    /// <param name="makerAdjustment">Maker adjustment (100 for 0.01%)</param>
    /// <param name="takerAdjustment">Taker adjustment (100 for 0.01%)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Sub account coin futures commission result</returns>
    Task<RestCallResult<BinanceLinkSubAccountCoinFuturesCommission>> SetCoinFuturesCommissionAdjustmentAsync(string subAccountId, string pair, int makerAdjustment, int takerAdjustment, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Sub Account COIN-Ⓜ Futures Commission Adjustment
    /// <para>The sub-account's COIN-Ⓜ futures commission of a symbol equals to the base commission of the symbol on the sub-account's fee tier plus the commission adjustment</para>
    /// <para>If pair not sent, commission adjustment of all symbols will be returned</para>
    /// <para>If futures disabled, it is not allowed to set subaccount's COIN-Ⓜ futures commission adjustment on any symbol</para>
    /// <para>Different symbols have the same commission for the same pair</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="pair">Pair</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Sub account coin futures commissions result</returns>
    Task<RestCallResult<List<BinanceLinkSubAccountFuturesCommission>>> GetCoinFuturesCommissionAdjustmentAsync(string subAccountId, string? pair = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Broker Commission Rebate Recent Record (Spot)
    /// <para>Only get the latest history of past 7 days</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="startDate">From date</param>
    /// <param name="endDate">To date</param>
    /// <param name="page">Page (default 1)</param>
    /// <param name="size">Size (default 500, max 500)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Rebates history</returns>
    Task<RestCallResult<List<BinanceLinkRebate>>> GetBrokerCommissionRebatesAsync(string subAccountId, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Broker Futures Commission Rebate Record
    /// </summary>
    /// <param name="futuresType">Futures type</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <param name="page">Page (default 1)</param>
    /// <param name="size">Size (default 10, max 100)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Rebate records</returns>
    Task<RestCallResult<List<BinanceLinkFuturesRebate>>> GetBrokerFuturesCommissionRebatesAsync(BinanceFuturesType futuresType, DateTime startDate, DateTime endDate, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);
}