namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientCoin
{
    public Task<RestCallResult<List<BinanceFuturesCoinAccountBalance>>> GetBalancesAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceFuturesCoinAccountBalance>>(GetUrl(dapi, v1, "balance"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceFuturesAccountUserCommissionRate>> GetUserCommissionRateAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol}
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesAccountUserCommissionRate>(GetUrl(dapi, v1, "commissionRate"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<BinanceFuturesCoinAccountInfo>> GetAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesCoinAccountInfo>(GetUrl(dapi, v1, "account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceFuturesPairBracket>>> GetPairBracketsAsync(string? pair = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("pair", pair);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceFuturesPairBracket>>(GetUrl(dapi, v1, "leverageBracket"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesSymbolBracket>>> GetBracketsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceFuturesSymbolBracket>>(GetUrl(dapi, v2, "leverageBracket"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 2 : 1);
    }

    public Task<RestCallResult<BinanceFuturesPositionMode>> GetPositionModeAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesPositionMode>(GetUrl(dapi, v1, "positionSide/dual"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 30);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinIncomeHistory>>> GetIncomeHistoryAsync(string? symbol = null, BinanceFuturesCoinIncomeType? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, long? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (incomeType.HasValue && !Enum.IsDefined(typeof(BinanceFuturesCoinIncomeType), incomeType.Value))
            throw new ArgumentOutOfRangeException(nameof(incomeType), incomeType, "Unsupported COIN-M income type");

        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptionalEnum("incomeType", incomeType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("page", page);
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceFuturesCoinIncomeHistory>>(GetUrl(dapi, v1, "income"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddMilliseconds("startTime", startTime);
        parameters.AddMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadIdInfo>(GetUrl(dapi, v1, "income/asyn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1000);
    }

    public Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", ValidateDownloadId(downloadId) }
        };
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadLink>(GetUrl(dapi, v1, "income/asyn/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForOrderHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddMilliseconds("startTime", startTime);
        parameters.AddMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadIdInfo>(GetUrl(dapi, v1, "order/asyn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1000);
    }

    public Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForOrderHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", ValidateDownloadId(downloadId) }
        };
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadLink>(GetUrl(dapi, v1, "order/asyn/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTradeHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddMilliseconds("startTime", startTime);
        parameters.AddMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadIdInfo>(GetUrl(dapi, v1, "trade/asyn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1000);
    }

    public Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTradeHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", ValidateDownloadId(downloadId) }
        };
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesDownloadLink>(GetUrl(dapi, v1, "trade/asyn/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    private static string ValidateDownloadId(string downloadId)
    {
        if (string.IsNullOrWhiteSpace(downloadId))
            throw new ArgumentException("downloadId cannot be null or whitespace", nameof(downloadId));

        return downloadId;
    }
}
