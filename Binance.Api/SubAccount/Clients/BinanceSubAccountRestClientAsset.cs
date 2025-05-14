namespace Binance.Api.SubAccount;

internal partial class BinanceSubAccountRestClient
{
    public Task<RestCallResult<BinanceSubAccountTransactionId>> FuturesTransferAsync(string email, string asset, decimal quantity, BinanceSubAccountFuturesTransferType type, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            { "email", email },
            { "asset", asset },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddEnum("type", type);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountTransactionId>(GetUrl(sapi, v1, "sub-account/futures/transfer"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountFuturesDetails>> GetFuturesDetailsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new ParameterCollection
        {
            { "email", email }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountFuturesDetails>(GetUrl(sapi, v1, "sub-account/futures/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountFuturesDetailsV2>> GetFuturesDetailsAsync(BinanceSubAccountFuturesType futuresType, string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new ParameterCollection
        {
            { "email", email },
        };
        parameters.AddEnum("futuresType", futuresType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountFuturesDetailsV2>(GetUrl(sapi, v2, "sub-account/futures/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountMarginDetails>> GetMarginDetailsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new ParameterCollection
        {
            { "email", email }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountMarginDetails>(GetUrl(sapi, v1, "sub-account/margin/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceSubAccountDepositAddress>> GetDepositAddressAsync(string email, string asset, string? network = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            { "email", email },
            { "coin", asset }
        };

        parameters.AddOptional("network", network);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountDepositAddress>(GetUrl(sapi, v1, "capital/deposit/subAddress"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceSubAccountDeposit>>> GetDepositHistoryAsync(string email, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new ParameterCollection
        {
            { "email", email }
        };

        parameters.AddOptional("coin", asset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("offset", offset?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceSubAccountDeposit>>(GetUrl(sapi, v1, "capital/deposit/subHisrec"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountFuturesSummary>> GetFuturesSummaryAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountFuturesSummary>(GetUrl(sapi, v1, "sub-account/futures/accountSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<BinanceSubAccountFuturesSummary>> GetFuturesSummaryAsync(BinanceSubAccountFuturesType futuresType, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("futuresType", futuresType);
        parameters.AddOptional("page", page);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceSubAccountFuturesSummaryV2>(GetUrl(sapi, v2, "sub-account/futures/accountSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
        return result.Success ? result.As(result.Data.Payload) : result.As<BinanceSubAccountFuturesSummary>(default!);
    }

    public Task<RestCallResult<BinanceSubAccountMarginSummary>> GetMarginSummaryAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountMarginSummary>(GetUrl(sapi, v1, "sub-account/margin/accountSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceSubAccountTransactionId>> MarginTransferAsync(string email, string asset, decimal quantity, BinanceSubAccountMarginTransferType type, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            { "email", email },
            { "asset", asset },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddEnum("type", type);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountTransactionId>(GetUrl(sapi, v1, "sub-account/margin/transfer"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<IEnumerable<BinanceSubAccountBalance>>> GetBalancesAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new ParameterCollection
        {
            { "email", email }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceSubAccountBalanceContainer>(GetUrl(sapi, v4, "sub-account/assets"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 60).ConfigureAwait(false);
        return result.Success ? result.As(result.Data.Payload) : result.As<IEnumerable<BinanceSubAccountBalance>>([]);
    }

    public Task<RestCallResult<BinanceSubAccountFuturesTransferHistory>> GetFuturesTransferHistoryAsync(string email, BinanceSubAccountFuturesType futuresType, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "email", email },
        };
        parameters.AddEnum("futuresType", futuresType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountFuturesTransferHistory>(GetUrl(sapi, v1, "sub-account/futures/internalTransfer"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceSubAccountSpotTransfer>>> GetSpotTransferHistoryAsync(string? fromEmail = null, string? toEmail = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("fromEmail", fromEmail);
        parameters.AddOptional("toEmail", toEmail);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceSubAccountSpotTransfer>>(GetUrl(sapi, v1, "sub-account/sub/transfer/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountSpotSummary>> GetSpotSummaryAsync(string? email = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("email", email);
        parameters.AddOptional("page", page);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountSpotSummary>(GetUrl(sapi, v1, "sub-account/spotSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<IEnumerable<BinanceSubAccountUniversalTransfer>>> GetUniversalTransferHistoryAsync(string? fromEmail = null, string? toEmail = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("fromEmail", fromEmail);
        parameters.AddOptional("toEmail", toEmail);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceSubAccountUniversalTransfersContainer>(GetUrl(sapi, v1, "sub-account/universalTransfer"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        return result.Success ? result.As(result.Data.Payload) : result.As<IEnumerable<BinanceSubAccountUniversalTransfer>>([]);
    }

    public Task<RestCallResult<BinanceSubAccountTransactionId>> FuturesAssetTransferAsync(string fromEmail, string toEmail, BinanceSubAccountFuturesType futuresType, string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            { "fromEmail", fromEmail },
            { "toEmail", toEmail },
            { "asset", asset },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddEnum("futuresType", futuresType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountTransactionId>(GetUrl(sapi, v1, "sub-account/futures/internalTransfer"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceSubAccountTransferSubAccount>>> GetTransferHistoryAsync(string? asset = null, BinanceSubAccountTransferType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceSubAccountTransferSubAccount>>(GetUrl(sapi, v1, "sub-account/transfer/subUserHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountTransactionId>> TransferSubAccountToMasterAsync(string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            { "asset", asset },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountTransactionId>(GetUrl(sapi, v1, "sub-account/transfer/subToMaster"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountTransactionId>> TransferSubAccountToSubAccountAsync(string email, string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            { "toEmail", email },
            { "asset", asset },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountTransactionId>(GetUrl(sapi, v1, "sub-account/transfer/subToSub"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountTransactionId>> UniversalTransferAsync(BinanceSubAccountTransferAccountType fromAccountType, BinanceSubAccountTransferAccountType toAccountType, string asset, decimal quantity, string? fromEmail = null, string? toEmail = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(fromEmail) && string.IsNullOrEmpty(toEmail)) throw new ArgumentException("fromEmail and/or toEmail should be provided");
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            { "asset", asset },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddEnum("fromAccountType", fromAccountType);
        parameters.AddEnum("toAccountType", toAccountType);
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("fromEmail", fromEmail);
        parameters.AddOptional("toEmail", toEmail);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountTransactionId>(GetUrl(sapi, v1, "sub-account/universalTransfer"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}