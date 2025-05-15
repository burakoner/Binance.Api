namespace Binance.Api.Broker;

/// <summary>
/// Interface for the Binance Exchange Link - Asset Rest API client.
/// </summary>
public interface IBinanceBrokerRestClientExchangeLinkAsset
{
    /// <summary>
    /// Sub Account Transfer (Spot)
    /// <para>You need to enable "internal transfer" option for the api key which requests this endpoint</para>
    /// <para>Transfer from master account if fromId not sent</para>
    /// <para>Transfer to master account if toId not sent</para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="fromId">From id</param>
    /// <param name="toId">To id</param>
    /// <param name="clientTransferId">Client transfer id, must be unique</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Transfer result</returns>
    Task<RestCallResult<BinanceBrokerTransferResult>> TransferAsync(string asset, decimal quantity, string? fromId, string? toId, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Sub Account Transfer History (Spot)
    /// <para>If showAllStatus is true, the status in response will show four types: INIT,PROCESS,SUCCESS,FAILURE</para>
    /// <para>If showAllStatus is false, the status in response will show three types: INIT,PROCESS,SUCCESS</para>
    /// </summary>
    /// <param name="fromId">From id</param>
    /// <param name="toId">To id</param>
    /// <param name="clientTransferId">Client transfer id</param>
    /// <param name="startDate">From date</param>
    /// <param name="endDate">To date</param>
    /// <param name="page">Page</param>
    /// <param name="limit">Limit (default 500, max 500)</param>
    /// <param name="showAllStatus">Show all status</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Transfer history</returns>
    Task<RestCallResult<List<BinanceBrokerTransferTransaction>>> GetTransfersAsync(string? fromId = null, string? toId = null, string? clientTransferId = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, bool showAllStatus = false, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Sub Account Transfer (Futures)
    /// <para>You need to enable "internal transfer" option for the api key which requests this endpoint</para>
    /// <para>Transfer from master account if fromId not sent</para>
    /// <para>Transfer to master account if toId not sent</para>
    /// <para>Each master account could transfer 5000 times/min</para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="futuresType">Futures type</param>
    /// <param name="fromId">From id</param>
    /// <param name="toId">To id</param>
    /// <param name="clientTransferId">Client transfer id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Transfer result</returns>
    Task<RestCallResult<BinanceBrokerTransferFuturesResult>> FuturesTransferAsync(string asset, decimal quantity, BinanceFuturesType futuresType, string? fromId, string? toId, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Sub Account Transfer History (Futures)
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="futuresType">Futures type</param>
    /// <param name="startDate">From date (default 30 days records)</param>
    /// <param name="endDate">To date (default 30 days records)</param>
    /// <param name="page">Page (default 1)</param>
    /// <param name="limit">Limit (default 50, max 500)</param>
    /// <param name="clientTransferId">Client transfer id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Transfer history</returns>
    Task<RestCallResult<BinanceBrokerageTransferFuturesTransactions>> GetFuturesTransfersAsync(string subAccountId, BinanceFuturesType futuresType, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Sub Account Deposit History
    /// <para>Please notice the default startDate and endDate to make sure that time interval is within 0-7 days</para>
    /// <para>If both startDate and endDate are sent, time between startDate and endDate must be less than 7 days</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="asset">Coin</param>
    /// <param name="status">Status</param>
    /// <param name="startDate">From date (default 7 days from current timestamp)</param>
    /// <param name="endDate">To date (default present timestamp)</param>
    /// <param name="limit">Limit (default 500)</param>
    /// <param name="offset">Offset (default 0)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceBrokerSubAccountDepositTransaction>>> GetDepositsAsync(string? subAccountId = null, string? asset = null, BinanceDepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Get Sub Account Deposit History V2

    /// <summary>
    /// Query Sub Account Spot Asset info
    /// <para>If subAccountId is not sent, the size must be sent</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="page">Page (default 1)</param>
    /// <param name="size">Size (default 10, max 20)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Asset info</returns>
    Task<RestCallResult<BinanceBrokerSpotAssetInfo>> GetSpotAssetInfoAsync(string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Sub Account Margin Asset info
    /// <para>If subAccountId is not sent, the size must be sent</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="page">Page (default 1)</param>
    /// <param name="size">Size (default 10, max 20)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Asset info</returns>
    Task<RestCallResult<BinanceBrokerMarginAssetInfo>> GetMarginAssetInfoAsync(string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Sub Account Futures Asset info
    /// <para>If subAccountId is not sent, the size must be sent</para>
    /// </summary>
    /// <param name="futuresType">Futures type</param>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="page">Page (default 1)</param>
    /// <param name="size">Size (default 10, max 20)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Asset info</returns>
    Task<RestCallResult<BinanceBrokerFuturesAssetInfo>> GetFuturesAssetInfoAsync(BinanceFuturesType futuresType, string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Query Sub Account Futures Asset info (V3)

    /// <summary>
    /// Sub Account Transfer Universal
    /// <para>You need to enable "internal transfer" option for the api key which requests this endpoint</para>
    /// <para>Transfer from master account if fromId not sent</para>
    /// <para>Transfer to master account if toId not sent</para>
    /// <para>Transfer between futures account is not supported</para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="fromId">From id</param>
    /// <param name="fromAccountType">From type</param>
    /// <param name="toId">To id</param>
    /// <param name="toAccountType">To type</param>
    /// <param name="clientTransferId">Client transfer id, must be unique</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Transfer result</returns>
    Task<RestCallResult<BinanceBrokerTransferResult>> UniversalTransferAsync(string asset, decimal quantity, string? fromId, BinanceBrokerAccountType fromAccountType, string? toId, BinanceBrokerAccountType toAccountType, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Sub Account Transfer History Universal
    /// <para>Either fromId or toId must be sent. Return fromId equal master account by default</para>
    /// <para>Only get the latest history of past 30 days</para>
    /// <para>If showAllStatus is true, the status in response will show four types: INIT,PROCESS,SUCCESS,FAILURE</para>
    /// </summary>
    /// <param name="fromId">From id</param>
    /// <param name="toId">To id</param>
    /// <param name="clientTransferId">Client transfer id</param>
    /// <param name="startDate">From date</param>
    /// <param name="endDate">To date</param>
    /// <param name="page">Page</param>
    /// <param name="limit">Limit (default 500, max 500)</param>
    /// <param name="showAllStatus">Show all status</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Transfer history</returns>
    Task<RestCallResult<List<BinanceBrokerTransferTransactionUniversal>>> GetUniversalTransfersAsync(string? fromId = null, string? toId = null, string? clientTransferId = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, bool showAllStatus = false, int? receiveWindow = null, CancellationToken ct = default);
}