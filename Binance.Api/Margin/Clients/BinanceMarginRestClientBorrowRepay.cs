using Binance.Api.Wallet;

namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{
    public Task<RestCallResult<List<BinanceMarginInterestRate>>> GetFutureHourlyInterestRateAsync(IEnumerable<string> assets, bool isolated, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "assets", string.Join(",", assets) },
            { "isIsolated", isolated.ToString().ToUpper() }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginInterestRate>>(GetUrl(sapi, v1, "margin/next-hourly-interest-rate"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceMarginInterestHistory>>> GetMarginInterestHistoryAsync(string? asset = null, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, bool? archived = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("size", limit);
        parameters.AddOptional("current", page);
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptional("archived", archived);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceMarginInterestHistory>>(GetUrl(sapi, v1, "margin/interestHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceWalletTransaction>> BorrowAsync(string asset, decimal quantity, bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        if (isIsolated == true && symbol == null)
            throw new ArgumentException("Symbol should be specified when using isolated margin");

        var parameters = new ParameterCollection
        {
            { "asset", asset },
            { "type", "BORROW" },
            { "amount", quantity.ToString(BinanceConstants.CI) }
        };
        parameters.AddOptional("isIsolated", isIsolated?.ToString().ToLower());
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceWalletTransaction>(GetUrl(sapi, v1, "margin/borrow-repay"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceWalletTransaction>> RepayAsync(string asset, decimal quantity, bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        var parameters = new ParameterCollection
            {
                { "asset", asset },
                { "type", "REPAY" },
                { "amount", quantity.ToString(BinanceConstants.CI) }
            };
        parameters.AddOptional("isIsolated", isIsolated?.ToString().ToLower());
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceWalletTransaction>(GetUrl(sapi, v1, "margin/borrow-repay"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceLoan>>> GetMarginLoansAsync(string asset, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? current = 1, int? limit = 10, string? isolatedSymbol = null, bool? archived = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        limit?.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new ParameterCollection
            {
                { "asset", asset },
                { "type", "BORROW" }
            };
        parameters.AddOptional("txId", transactionId);
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);

        // TxId or startTime must be sent. txId takes precedence.
        if (!transactionId.HasValue) parameters.AddOptionalMilliseconds("startTime", startTime ?? DateTime.MinValue);
        else parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("size", limit);
        parameters.AddOptional("archived", archived);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceLoan>>(GetUrl(sapi, v1, "margin/borrow-repay"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceMarginInterestRateHistory>>> GetMarginInterestRateHistoryAsync(string asset, string? vipLevel = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset?.ValidateNotNull(nameof(asset));
        limit?.ValidateIntBetween(nameof(limit), 1, 100);

        var parameters = new ParameterCollection
        {
            { "asset", asset! }
        };
        parameters.AddOptional("vipLevel", vipLevel);
        parameters.AddOptional("size", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginInterestRateHistory>>(GetUrl(sapi, v1, "margin/interestRateHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceMarginAmount>> GetMarginMaxBorrowAmountAsync(string asset, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            { "asset", asset }
        };
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginAmount>(GetUrl(sapi, v1, "margin/maxBorrowable"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }
}