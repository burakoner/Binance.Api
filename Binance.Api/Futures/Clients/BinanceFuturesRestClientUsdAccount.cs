namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientUsd
{
    public Task<RestCallResult<IEnumerable<BinanceUsdFuturesAccountBalance>>> GetBalancesAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceUsdFuturesAccountBalance>>(GetUrl(fapi, v3, "balance"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    // TODO: Account Information V3(USER_DATA)

    public Task<RestCallResult<BinanceFuturesAccountInfo>> GetAccountInfoV2Async(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesAccountInfo>(GetUrl(fapi, v2, "account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceFuturesAccountUserCommissionRate>> GetUserCommissionRateAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol}
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesAccountUserCommissionRate>(GetUrl(fapi, v1, "commissionRate"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<BinanceFuturesAccountConfiguration>> GetAccountConfigurationAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesAccountConfiguration>(GetUrl(fapi, v1, "accountConfig"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesSymbolConfiguration>>> GetSymbolConfigurationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceFuturesSymbolConfiguration>>(GetUrl(fapi, v1, "symbolConfig"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<IEnumerable<BinanceRateLimit>>> GetOrderRateLimitAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceRateLimit>>(GetUrl(fapi, v1, "rateLimit/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesSymbolBracket>>> GetBracketsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceFuturesSymbolBracket>>(GetUrl(fapi, v1, "leverageBracket"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceFuturesMultiAssetMode>> GetMultiAssetsModeAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesMultiAssetMode>(GetUrl(fapi, v1, "multiAssetsMargin"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 30);
    }

    public Task<RestCallResult<BinanceFuturesPositionMode>> GetPositionModeAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesPositionMode>(GetUrl(fapi, v1, "positionSide/dual"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 30);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesIncomeHistory>>> GetIncomeHistoryAsync(string? symbol = null, string? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("incomeType", incomeType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("page", page);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));

        return RequestAsync<IEnumerable<BinanceFuturesIncomeHistory>>(GetUrl(fapi, v1, "income"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 30);
    }

    public Task<RestCallResult<BinanceFuturesTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesTradingStatus>(GetUrl(fapi, v1, "apiTradingStatus"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadIdInfo>(GetUrl(fapi, v1, "income/asyn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1500);
    }

    public Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", downloadId }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadLink>(GetUrl(fapi, v1, "income/asyn/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForOrderHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadIdInfo>(GetUrl(fapi, v1, "order/asyn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1500);
    }

    public Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForOrderHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", downloadId }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadLink>(GetUrl(fapi, v1, "order/asyn/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTradeHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadIdInfo>(GetUrl(fapi, v1, "trade/asyn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1500);
    }

    public Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTradeHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", downloadId }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadLink>(GetUrl(fapi, v1, "trade/asyn/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public async Task<RestCallResult<bool>> SetBnbBurnStatusAsync(bool feeBurn, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "feeBurn", feeBurn.ToString() }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(GetUrl(fapi, v1, "feeBurn"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public Task<RestCallResult<BinanceBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBnbBurnStatus>(GetUrl(fapi, v1, "feeBurn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 30);
    }
}
