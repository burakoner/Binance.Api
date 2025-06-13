namespace Binance.Api.Link;

internal partial class BinanceLinkRestClientExchangeLink
{
    public Task<RestCallResult<BinanceLinkTransferResult>> TransferAsync(string asset, decimal quantity, string? fromId, string? toId, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
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

        return RequestAsync<BinanceLinkTransferResult>(GetUrl(sapi, v1, "broker/transfer"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceLinkTransferTransaction>>> GetTransfersAsync(string? fromId = null, string? toId = null, string? clientTransferId = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, bool showAllStatus = false, int? receiveWindow = null, CancellationToken ct = default)
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

        return RequestAsync<List<BinanceLinkTransferTransaction>>(GetUrl(sapi, v1, "broker/transfer"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceLinkTransferFuturesResult>> FuturesTransferAsync(string asset, decimal quantity, BinanceFuturesType futuresType, string? fromId, string? toId, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
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

        return RequestAsync<BinanceLinkTransferFuturesResult>(GetUrl(sapi, v1, "broker/transfer/futures"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceLinkTransferFuturesTransactions>> GetFuturesTransfersAsync(string subAccountId, BinanceFuturesType futuresType, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
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

        return RequestAsync<BinanceLinkTransferFuturesTransactions>(GetUrl(sapi, v1, "broker/transfer/futures"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceLinkSubAccountDepositTransaction>>> GetDepositsAsync(string? subAccountId = null, string? asset = null, BinanceDepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default)
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

        return RequestAsync<List<BinanceLinkSubAccountDepositTransaction>>(GetUrl(sapi, v1, "broker/subAccount/depositHist"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceLinkSpotAssetInfo>> GetSpotAssetInfoAsync(string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("subAccountId", subAccountId);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("size", size?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkSpotAssetInfo>(GetUrl(sapi, v1, "broker/subAccount/spotSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceLinkMarginAssetInfo>> GetMarginAssetInfoAsync(string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("subAccountId", subAccountId);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("size", size?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkMarginAssetInfo>(GetUrl(sapi, v1, "broker/subAccount/marginSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceLinkFuturesAssetInfo>> GetFuturesAssetInfoAsync(BinanceFuturesType futuresType, string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("futuresType", futuresType);
        parameters.AddOptional("subAccountId", subAccountId);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("size", size?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkFuturesAssetInfo>(GetUrl(sapi, v2, "subAccount/futuresSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceLinkTransferResult>> UniversalTransferAsync(string asset, decimal quantity, string? fromId, BinanceLinkAccountType fromAccountType, string? toId, BinanceLinkAccountType toAccountType, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
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

        return RequestAsync<BinanceLinkTransferResult>(GetUrl(sapi, v1, "broker/universalTransfer"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceLinkTransferTransactionUniversal>>> GetUniversalTransfersAsync(string? fromId = null, string? toId = null, string? clientTransferId = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, bool showAllStatus = false, int? receiveWindow = null, CancellationToken ct = default)
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

        return RequestAsync<List<BinanceLinkTransferTransactionUniversal>>(GetUrl(sapi, v1, "broker/universalTransfer"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }
}