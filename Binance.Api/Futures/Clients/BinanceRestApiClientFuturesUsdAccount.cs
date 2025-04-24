using Binance.Net.Objects.Models.Futures;

namespace Binance.Api.Futures;

internal partial class BinanceRestApiClientFuturesUsd
{
    /*

    #region Future Account Balance

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceUsdFuturesAccountBalance>>> GetBalancesAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v3/balance", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceUsdFuturesAccountBalance>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    // TODO: Account Information V3(USER_DATA)


    #region Get Account Info

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesAccountInfo>> GetAccountInfoV2Async(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v2/account", BinanceExchange.RateLimiter.FuturesRest, 10, true);
        return await _baseClient.SendAsync<BinanceFuturesAccountInfo>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion



    #region Future Account User Commission Rate
    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesAccountUserCommissionRate>> GetUserCommissionRateAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol}
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/commissionRate", BinanceExchange.RateLimiter.FuturesRest, 20, true);
        return await _baseClient.SendAsync<BinanceFuturesAccountUserCommissionRate>(request, parameters, ct).ConfigureAwait(false);
    }
    #endregion


    #region Get Account Configuration

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesAccountConfiguration>> GetAccountConfigurationAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v1/accountConfig", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        var result = await _baseClient.SendAsync<BinanceFuturesAccountConfiguration>(request, parameters, ct).ConfigureAwait(false);
        return result;
    }

    #endregion

    #region Get Symbol Configuration

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceSymbolConfiguration>>> GetSymbolConfigurationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v1/symbolConfig", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceSymbolConfiguration>>(request, parameters, ct).ConfigureAwait(false);
        return result;
    }

    #endregion

    #region Get Order Rate Limit

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceRateLimit>>> GetOrderRateLimitAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/rateLimit/order", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceRateLimit>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion


    #region Notional and Leverage Brackets

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesSymbolBracket>>> GetBracketsAsync(string? symbolOrPair = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbolOrPair);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/leverageBracket", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesSymbolBracket>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Multi assets mode

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesMultiAssetMode>> GetMultiAssetsModeAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/multiAssetsMargin", BinanceExchange.RateLimiter.FuturesRest, 30, true);
        return await _baseClient.SendAsync<BinanceFuturesMultiAssetMode>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Get Current Position Mode

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesPositionMode>> GetPositionModeAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/positionSide/dual", BinanceExchange.RateLimiter.FuturesRest, 30, true);
        return await _baseClient.SendAsync<BinanceFuturesPositionMode>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Get Income History

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesIncomeHistory>>> GetIncomeHistoryAsync(string? symbol = null, string? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("incomeType", incomeType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("page", page);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/income", BinanceExchange.RateLimiter.FuturesRest, 30, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesIncomeHistory>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Trading status
    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/apiTradingStatus", BinanceExchange.RateLimiter.FuturesRest, 10, true);
        return await _baseClient.SendAsync<BinanceFuturesTradingStatus>(request, parameters, ct).ConfigureAwait(false);

    }
    #endregion

    #region Get download id for transaction history
    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/income/asyn", BinanceExchange.RateLimiter.FuturesRest, 1500, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadIdInfo>(request, parameters, ct).ConfigureAwait(false);
    }
    #endregion

    #region Download transaction history
    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", downloadId }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/income/asyn/id", BinanceExchange.RateLimiter.FuturesRest, 10, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadLink>(request, parameters, ct).ConfigureAwait(false);
    }
    #endregion

    #region Get download id for transaction history
    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForOrderHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/order/asyn", BinanceExchange.RateLimiter.FuturesRest, 1500, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadIdInfo>(request, parameters, ct).ConfigureAwait(false);
    }
    #endregion

    #region Download order history
    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForOrderHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", downloadId }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/order/asyn/id", BinanceExchange.RateLimiter.FuturesRest, 10, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadLink>(request, parameters, ct).ConfigureAwait(false);
    }
    #endregion

    #region Get download id for trade history
    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTradeHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/trade/asyn", BinanceExchange.RateLimiter.FuturesRest, 1500, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadIdInfo>(request, parameters, ct).ConfigureAwait(false);
    }
    #endregion

    #region Download trade history
    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTradeHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", downloadId }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/trade/asyn/id", BinanceExchange.RateLimiter.FuturesRest, 10, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadLink>(request, parameters, ct).ConfigureAwait(false);
    }
    #endregion

    #region Set BNB Burn Status

    /// <inheritdoc />
    public async Task<WebCallResult> SetBnbBurnStatusAsync(bool feeBurn, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "feeBurn", feeBurn.ToString() }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/feeBurn", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Get BNB Burn Status

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/feeBurn", BinanceExchange.RateLimiter.FuturesRest, 30, true);
        return await _baseClient.SendAsync<BinanceBnbBurnStatus>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion



    */


















}