namespace Binance.Api.Wallet;

internal partial class BinanceWalletRestApiClient
{
    public Task<RestCallResult<BinanceVipLevelAndStatus>> GetAccountVipLevelAndStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceVipLevelAndStatus>(GetUrl(sapi, v1, "account/info"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceSpotAccountSnapshot>>> GetDailySpotAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null,
        CancellationToken ct = default)
        => GetDailyAccountSnapshot<IEnumerable<BinanceSpotAccountSnapshot>>(BinanceAccountType.Spot, startTime, endTime, limit, receiveWindow, ct);

    public Task<RestCallResult<IEnumerable<BinanceMarginAccountSnapshot>>> GetDailyMarginAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null,
        CancellationToken ct = default)
        => GetDailyAccountSnapshot<IEnumerable<BinanceMarginAccountSnapshot>>(BinanceAccountType.Margin, startTime, endTime, limit, receiveWindow, ct);

    public Task<RestCallResult<IEnumerable<BinanceFuturesAccountSnapshot>>> GetDailyFutureAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null,
        CancellationToken ct = default)
        => GetDailyAccountSnapshot<IEnumerable<BinanceFuturesAccountSnapshot>>(BinanceAccountType.Futures, startTime, endTime, limit, receiveWindow, ct);

    private async Task<RestCallResult<T>> GetDailyAccountSnapshot<T>(BinanceAccountType accountType,
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null,
        CancellationToken ct = default) where T : class
    {
        limit?.ValidateIntBetween(nameof(limit), 7, 30);

        var parameters = new ParameterCollection();
        parameters.AddEnum("type", accountType);
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceSnapshotWrapper<T>>(GetUrl(sapi, v1, "accountSnapshot"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 2400).ConfigureAwait(false);
        if (!result.Success) return result.As<T>(default!);
        if (result.Data.Code != 200) return result.AsError<T>(new ServerError(result.Data.Code, result.Data.Message!));

        return result.As(result.Data.SnapshotData);
    }

    public async Task<RestCallResult<bool>> DisableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "account/disableFastWithdrawSwitch"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<bool>> EnableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "account/enableFastWithdrawSwitch"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public Task<RestCallResult<BinanceAccountStatus>> GetAccountStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAccountStatus>(GetUrl(sapi, v1, "account/status"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<BinanceTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceResult<BinanceTradingStatus>>(GetUrl(sapi, v1, "account/apiTradingStatus"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (!result) return result.As<BinanceTradingStatus>(default!);
        return !string.IsNullOrEmpty(result.Data.Message) ? result.AsError<BinanceTradingStatus>(new ServerError(result.Data.Message!)) : result.As(result.Data.Data);
    }

    public Task<RestCallResult<BinanceAPIKeyPermissions>> GetAPIKeyPermissionsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAPIKeyPermissions>(GetUrl(sapi, v1, "account/apiRestrictions"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}