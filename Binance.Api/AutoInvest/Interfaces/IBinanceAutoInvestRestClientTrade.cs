namespace Binance.Api.AutoInvest;

/// <summary>
/// Interface for the Binance Auto Invest Trade Rest API client.
/// </summary>
public interface IBinanceAutoInvestRestClientTrade
{
    /// <summary>
    /// Make a one time transaction
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade" /></para>
    /// </summary>
    /// <param name="sourceType">The source type, "MAIN_SITE" for normal, "TR" for Turkey users</param>
    /// <param name="requestId">Request id</param>
    /// <param name="subscriptionQuantity">The quantity to subscribe</param>
    /// <param name="sourceAsset">The source asset</param>
    /// <param name="flexibleAllowedToUse">true: use flexible wallet</param>
    /// <param name="indexId">Index id</param>
    /// <param name="subscriptionDetails">Subscription details of asset => percentage. Total percentage should add up to 100%</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestTradeResult>> OneTimeTransactionAsync(string sourceType, string requestId, decimal subscriptionQuantity, string sourceAsset, bool flexibleAllowedToUse, long indexId, Dictionary<string, decimal> subscriptionDetails, CancellationToken ct = default);

    /// <summary>
    /// Edit the status of a plan
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade/Change-Plan-Status" /></para>
    /// </summary>
    /// <param name="planId">The plan id</param>
    /// <param name="status">New status</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestEditStatusResult>> SetPlanStatusAsync(long planId, BinanceAutoInvestPlanStatus status, CancellationToken ct = default);

    /// <summary>
    /// Edit a plan
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade/Investment-plan-adjustment" /></para>
    /// </summary>
    /// <param name="planId">The plan id</param>
    /// <param name="subscriptionQuantity">The quantity</param>
    /// <param name="subscriptionCycle">The cycle</param>
    /// <param name="subscriptionStartDay">Start day, 1..31. Required if cycle is monthly</param>
    /// <param name="subscriptionStartWeekday">Start weekday, required if cycle is Weekly or Bi-Weekly</param>
    /// <param name="subscriptionStartTime">Start hour, 1..24</param>
    /// <param name="sourceAsset">Source asset</param>
    /// <param name="flexibleAllowedToUse">True:use flexible wallet</param>
    /// <param name="subscriptionDetails">Subscription details of asset => percentage. Total percentage should add up to 100%</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestEditResult>> SetPlanAsync(string planId, decimal subscriptionQuantity, AutoInvestSubscriptionCycle subscriptionCycle, string sourceAsset, Dictionary<string, decimal> subscriptionDetails, int? subscriptionStartDay = null, string? subscriptionStartWeekday = null, int? subscriptionStartTime = null, bool? flexibleAllowedToUse = null, CancellationToken ct = default);

    /// <summary>
    /// Redeem index linked plan
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade/Index-Linked-Plan-Redemption" /></para>
    /// </summary>
    /// <param name="indexId">The index id</param>
    /// <param name="requestId">Request id</param>
    /// <param name="redemptionPercentage">Redemption percentage</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestRedemptionResult>> RedeemAsync(string indexId, string requestId, int redemptionPercentage, CancellationToken ct = default);

    /// <summary>
    /// Get subscription transaction history
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade/Query-subscription-transaction-history" /></para>
    /// </summary>
    /// <param name="planId">Filter by plan id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="targetAsset">Filter by target asset</param>
    /// <param name="planType">Filter by plan type</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceListTotalResponse<BinanceAutoInvestTransaction>>> GetSubscriptionHistoryAsync(long? planId = null, DateTime? startTime = null, DateTime? endTime = null, string? targetAsset = null, BinanceAutoInvestPlanType? planType = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

    /// <summary>
    /// Get one time transaction status
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade/Query-One-Time-Transaction-Status" /></para>
    /// </summary>
    /// <param name="transactionId">Transaction id</param>
    /// <param name="requestId">Request id</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestOneTimeTransaction>> GetOneTimeTransactionAsync(long transactionId, string requestId, CancellationToken ct = default);

    /// <summary>
    /// Create new investment plan
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade/Investment-plan-creation" /></para>
    /// </summary>
    /// <param name="sourceType">Source type, "MAIN_SITE" for normal, "TR" for Turkey users</param>
    /// <param name="requestId">Request id</param>
    /// <param name="planType">Plan type</param>
    /// <param name="subscriptionQuantity">Subscription quantity</param>
    /// <param name="subscriptionCycle">Subscription cycle</param>
    /// <param name="subscriptionStartDay">Subscription start day, 1..31. Required when cycle is montly</param>
    /// <param name="subscriptionStartWeekday">Subscription start weekday, "MON" .. "SUN". Required when cycle is Weekly or BiWeekly</param>
    /// <param name="subscriptionStartTime">Subscription start time hour, 0..24</param>
    /// <param name="sourceAsset">Source asset</param>
    /// <param name="flexibleAllowedToUse">True: flexible wallet</param>
    /// <param name="subscriptionDetails">Subscription details of asset => percentage. Total percentage should add up to 100%</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestTradeResult>> CreatePlanAsync(string sourceType, BinanceAutoInvestPlanType planType, decimal subscriptionQuantity, AutoInvestSubscriptionCycle subscriptionCycle, int subscriptionStartTime, string sourceAsset, Dictionary<string, decimal> subscriptionDetails, string? requestId = null, int? subscriptionStartDay = null, string? subscriptionStartWeekday = null, bool? flexibleAllowedToUse = null, CancellationToken ct = default);

    /// <summary>
    /// Get index linked plan redemption history
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade/Query-Index-Linked-Plan-Redemption" /></para>
    /// </summary>
    /// <param name="requestId">Request id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="asset">The asset, for example `ETH`</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<List<BinanceAutoInvestRedemption>>> GetRedemptionHistoryAsync(long requestId, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, string? asset = null, CancellationToken ct = default);

    /// <summary>
    /// Get holding details of a plan
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade/Query-holding-details-of-the-plan" /></para>
    /// </summary>
    /// <param name="planId">Filter by plan id</param>
    /// <param name="requestId">Request id</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestHoldings>> GetHoldingsAsync(long? planId = null, string? requestId = null, CancellationToken ct = default);

    /// <summary>
    /// Get index linked plan position details
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade/Query-Index-Linked-Plan-Position-Details" /></para>
    /// </summary>
    /// <param name="indexId">The index id</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestIndexPlanPosition>> GetPositionAsync(long indexId, CancellationToken ct = default);

    /// <summary>
    /// Get index linked plan rebalance history
    /// <para><a href="https://developers.binance.com/docs/auto_invest/trade/Index-Linked-Plan-Rebalance-Details" /></para>
    /// </summary>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<List<BinanceAutoInvestRebalance>>> GetRebalanceHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default);
}