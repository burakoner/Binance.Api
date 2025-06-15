namespace Binance.Api.DualInvestment;

/// <summary>
/// Interface for the Binance Dual Investment -> Trade Rest API client.
/// </summary>
public interface IBinanceDualInvestmentRestClientTrade
{
    /// <summary>
    /// Subscribe Dual Investment products
    /// <para><a href="https://developers.binance.com/docs/dual_investment/trade" /></para>
    /// </summary>
    /// <param name="id">get id from /sapi/v1/dci/product/list</param>
    /// <param name="orderId">get orderId from /sapi/v1/dci/product/list</param>
    /// <param name="quantity">the amount for subscribing</param>
    /// <param name="autoCompoundPlan">NONE: switch off the plan, STANDARD:standard plan,ADVANCED:advanced plan</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceDualInvestmentSubscription>> SubscribeAsync(long id, long orderId, decimal quantity, BinanceDualInvestmentAutoCompoundPlan autoCompoundPlan, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Dual Investment positions (batch)
    /// <para><a href="https://developers.binance.com/docs/dual_investment/trade/Get-Dual-Investment-positions" /></para>
    /// </summary>
    /// <param name="status">PENDING:Products are purchasing, will give results later;PURCHASE_SUCCESS:purchase successfully;SETTLED: Products are finish settling;PURCHASE_FAIL:fail to purchase;REFUNDING:refund ongoing;REFUND_SUCCESS:refund to spot account successfully; SETTLING:Products are settling. If don't fill this field, will response all the position status.</param>
    /// <param name="pageSize">Default: 10, Max:100</param>
    /// <param name="pageIndex">Default:1</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceListTotalResponse<BinanceDualInvestmentPosition>>> GetPositionsAsync(BinanceDualInvestmentStatus? status = null, int? pageSize = null, int? pageIndex = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Check Dual Investment accounts
    /// <para><a href="https://developers.binance.com/docs/dual_investment/trade/Check-Dual-Investment-accounts" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceDualInvestmentAccount>> GetAccountAsync(CancellationToken ct = default);

    /// <summary>
    /// Change Auto-Compound status
    /// <para><a href="https://developers.binance.com/docs/dual_investment/trade/Change-Auto-Compound-status" /></para>
    /// </summary>
    /// <param name="positionId">Position Id</param>
    /// <param name="autoCompoundPlan">Auto Compound Plan</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceDualInvestmentPositionPlan>> SetAutoCompoundPlanAsync(long positionId, BinanceDualInvestmentAutoCompoundPlan autoCompoundPlan, int? receiveWindow = null, CancellationToken ct = default);
}