namespace Binance.Api.Broker;

internal partial class BinanceBrokerRestClientExchangeLink
{
    public Task<RestCallResult<BinanceBrokerTransferResult>> TransferAsync(string asset, decimal quantity, string? fromId, string? toId, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            {"asset", asset},
            {"amount", quantity.ToString(BinanceConstants.CI)},
        };
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("toId", toId);
        parameters.AddOptional("clientTranId", clientTransferId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerTransferResult>(GetUrl(sapi, v1, "broker/transfer"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceBrokerTransferTransaction>>> GetTransfersAsync(string? fromId = null, string? toId = null, string? clientTransferId = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, bool showAllStatus = false, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            {"showAllStatus", showAllStatus.ToString().ToLower()},
        };
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("toId", toId);
        parameters.AddOptional("clientTranId", clientTransferId);
        parameters.AddOptionalMilliseconds("startTime", startDate);
        parameters.AddOptionalMilliseconds("endTime", endDate);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceBrokerTransferTransaction>>(GetUrl(sapi, v1, "broker/transfer"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerTransferFuturesResult>> FuturesTransferAsync(string asset, decimal quantity, BinanceFuturesType futuresType, string? fromId, string? toId, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            {"asset", asset},
            {"amount", quantity.ToString(BinanceConstants.CI)},
        };
        parameters.AddEnum("futuresType", futuresType);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("toId", toId);
        parameters.AddOptional("clientTranId", clientTransferId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerTransferFuturesResult>(GetUrl(sapi, v1, "broker/transfer/futures"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerageTransferFuturesTransactions>> GetFuturesTransfersAsync(string subAccountId, BinanceFuturesType futuresType, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection()
        {
            {"subAccountId", subAccountId}
        };
        parameters.AddEnum("futuresType", futuresType);
        parameters.AddOptionalMilliseconds("startTime", startDate);
        parameters.AddOptionalMilliseconds("endTime", endDate);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));
        parameters.AddOptional("clientTranId", clientTransferId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerageTransferFuturesTransactions>(GetUrl(sapi, v1, "broker/transfer/futures"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceBrokerSubAccountDepositTransaction>>> GetDepositsAsync(string? subAccountId = null, string? asset = null, BinanceDepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("subAccountId", subAccountId);
        parameters.AddOptional("coin", asset);
        parameters.AddOptionalEnum("status", status);
        parameters.AddOptionalMilliseconds("startTime", startDate);
        parameters.AddOptionalMilliseconds("endTime", endDate);
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));
        parameters.AddOptional("offset", offset?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceBrokerSubAccountDepositTransaction>>(GetUrl(sapi, v1, "broker/subAccount/depositHist"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerSpotAssetInfo>> GetSpotAssetInfoAsync(string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("subAccountId", subAccountId);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("size", size?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerSpotAssetInfo>(GetUrl(sapi, v1, "broker/subAccount/spotSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerMarginAssetInfo>> GetMarginAssetInfoAsync(string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("subAccountId", subAccountId);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("size", size?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerMarginAssetInfo>(GetUrl(sapi, v1, "broker/subAccount/marginSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerFuturesAssetInfo>> GetFuturesAssetInfoAsync(BinanceFuturesType futuresType, string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("futuresType", futuresType);
        parameters.AddOptional("subAccountId", subAccountId);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("size", size?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerFuturesAssetInfo>(GetUrl(sapi, v2, "subAccount/futuresSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerTransferResult>> UniversalTransferAsync(string asset, decimal quantity, string? fromId, BinanceBrokerAccountType fromAccountType, string? toId, BinanceBrokerAccountType toAccountType, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection()
        {
            {"asset", asset},
            {"amount", quantity.ToString(BinanceConstants.CI)}
        };
        parameters.AddEnum("fromAccountType", fromAccountType);
        parameters.AddEnum("toAccountType", toAccountType);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("toId", toId);
        parameters.AddOptional("clientTranId", clientTransferId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerTransferResult>(GetUrl(sapi, v1, "broker/universalTransfer"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceBrokerTransferTransactionUniversal>>> GetUniversalTransfersAsync(string? fromId = null, string? toId = null, string? clientTransferId = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, bool showAllStatus = false, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            {"showAllStatus", showAllStatus.ToString().ToLower()},
        };
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("toId", toId);
        parameters.AddOptional("clientTranId", clientTransferId);
        parameters.AddOptionalMilliseconds("startTime", startDate);
        parameters.AddOptionalMilliseconds("endTime", endDate);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceBrokerTransferTransactionUniversal>>(GetUrl(sapi, v1, "broker/universalTransfer"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }
}